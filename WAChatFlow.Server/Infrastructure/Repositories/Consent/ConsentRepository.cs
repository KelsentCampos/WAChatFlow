using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using WAChatFlow.Server.Infrastructure.Data;

namespace WAChatFlow.Server.Infrastructure.Repositories.Consent
{
    public class ConsentRepository : IConsentRepository
    {
        private readonly DatabaseConnectionFactory _dbConnection;

        public ConsentRepository(DatabaseConnectionFactory dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<long> CreateAsync(long userId, string channel, string code, TimeSpan ttl, CancellationToken ct = default)
        {
            using var conn = await _dbConnection.CreateOpenConnectionAsync(ct);

            using var cmdExp = new SqlCommand(@"
                UPDATE tbl SET tbl.[Status] = 'EXPIRED' FROM dbo.UserConsentOtp tbl WHERE tbl.UserId = @UserId AND tbl.CommunicationChannel = @Channel AND tbl.[Status] = 'PENDING' AND tbl.ExpiresUtc <= SYSUTCDATETIME()
            ", (SqlConnection)conn);

            cmdExp.Parameters.Add("@UserId", SqlDbType.BigInt).Value = userId;
            cmdExp.Parameters.Add("@Channel", SqlDbType.VarChar, 32).Value = channel ?? string.Empty;

            await cmdExp.ExecuteNonQueryAsync();

            using var cmd = new SqlCommand(@"
                INSERT INTO dbo.UserConsentOtp (UserId, CommunicationChannel, CodeHash, Salt, ExpiresUtc, Attempts, MaxAttempts, [Status]) OUTPUT inserted.OtpId 
						VALUES (@UserId, @Channel, @CodeHash, @Salt, DATEADD(SECOND, @TtlSec, SYSUTCDATETIME()), 0, @MaxAttempts, 'PENDING') 
            ", (SqlConnection)conn);

            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = SHA256.HashData(CombineCode(salt, Encoding.UTF8.GetBytes(code)));

            cmd.Parameters.Add("@UserId", SqlDbType.BigInt).Value = userId;
            cmd.Parameters.Add("@Channel", SqlDbType.VarChar, 32).Value = channel;
            cmd.Parameters.Add("@CodeHash", SqlDbType.VarBinary, 32).Value = hash;
            cmd.Parameters.Add("@Salt", SqlDbType.VarBinary, 16).Value = salt;
            cmd.Parameters.Add("@TtlSec", SqlDbType.Int).Value = (int)ttl.TotalSeconds;
            cmd.Parameters.Add("@MaxAttempts", SqlDbType.Int).Value = 5;

            try
            {
                var result = await cmd.ExecuteScalarAsync();

                return (result is long l) ? l : Convert.ToInt64(result);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                {
                    return 0;
                }
            }
        }

        public async Task<bool> VerifyAndConsumeAsync(long userId, string channel, string code)
        {
            using var conn = await _dbConnection.CreateOpenConnectionAsync();

            using var tx = conn.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                using var sel = new SqlCommand(@"
                    SELECT TOP(1) otp.OtpId, otp.CodeHash, otp.Salt, otp.ExpiresUtc, otp.Attempts, otp.MaxAttempts
                        FROM	dbo.UserConsentOtp otp WITH (UPDLOCK, ROWLOCK)
                    WHERE	otp.UserId					= @UserId 
                        AND otp.CommunicationChannel	= @Channel 
                        AND otp.[Status] = 'PENDING'
                    ORDER BY otp.OtpId DESC
                ", (SqlConnection)conn, (SqlTransaction)tx);

                sel.Parameters.Add("@UserId", SqlDbType.BigInt).Value = userId;
                sel.Parameters.Add("@Channel", SqlDbType.VarChar, 32).Value = channel;

                long otpId;
                byte[] dbHash, salt;
                DateTime expiresUtc;
                int attempts, maxAttempts;

                using (var rd = await sel.ExecuteReaderAsync(CommandBehavior.SingleRow))
                {
                    if (!await rd.ReadAsync())
                    {
                        rd.Close();
                        sel.Dispose();
                        tx.Commit();
                        return false;
                    }

                    otpId = rd.GetInt64(rd.GetOrdinal("OtpId"));
                    dbHash = (byte[])rd["CodeHash"];
                    salt = (byte[])rd["Salt"];
                    expiresUtc = rd.GetDateTime(rd.GetOrdinal("ExpiresUtc"));
                    attempts = rd.GetInt32(rd.GetOrdinal("Attempts"));
                    maxAttempts = rd.GetInt32(rd.GetOrdinal("MaxAttempts"));
                }

                if (DateTime.UtcNow > expiresUtc || attempts >= maxAttempts)
                {
                    using var updExpire = new SqlCommand(
                        "UPDATE tbl SET tbl.[Status] = 'EXPIRED' FROM dbo.UserConsentOtp tbl WHERE tbl.OtpId = @IdOtp",
                        (SqlConnection)conn, (SqlTransaction)tx);

                    updExpire.Parameters.Add("@IdOtp", SqlDbType.BigInt).Value = otpId;

                    await updExpire.ExecuteNonQueryAsync();
                    tx.Commit();

                    return false;
                }

                byte[] inputHash = SHA256.HashData(CombineCode(salt, Encoding.UTF8.GetBytes(code)));

                bool codeOk = inputHash.SequenceEqual(dbHash);

                if (codeOk)
                {
                    using var updOk = new SqlCommand(@"
                        UPDATE tbl SET tbl.Attempts = tbl.Attempts + 1, tbl.[Status] = 'USED' FROM dbo.UserConsentOtp tbl WHERE tbl.OtpId = @IdOtp",
                        (SqlConnection)conn, (SqlTransaction)tx);

                    updOk.Parameters.Add("@IdOtp", SqlDbType.BigInt).Value = otpId;

                    await updOk.ExecuteNonQueryAsync();
                    tx.Commit();

                    return true;
                }
                else
                {
                    string newStatus = (attempts + 1 >= maxAttempts || DateTime.UtcNow > expiresUtc) ? "EXPIRED" : "PENDING";

                    using var updFail = new SqlCommand(@"
                        UPDATE tbl SET tbl.Attempts = tbl.Attempts + 1, tbl.[Status] = @Status FROM dbo.UserConsentOtp tbl WHERE tbl.OtpId = @IdOtp",
                        (SqlConnection)conn, (SqlTransaction)tx);

                    updFail.Parameters.Add("@IdOtp", SqlDbType.BigInt).Value = otpId;
                    updFail.Parameters.Add("@Status", SqlDbType.VarChar, 16).Value = newStatus;
                    await updFail.ExecuteNonQueryAsync();

                    tx.Commit();
                    return false;
                }
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public async Task MarkConsentValidatedAsync(long userId, string channel, string consentGrantedMessageId)
        {
            using var conn = await _dbConnection.CreateOpenConnectionAsync();

            using var cmd = new SqlCommand(@"
                UPDATE cons SET 
                		 cons.ConsentStatus					= 'VALIDATED'
                		,cons.ConsentGrantedUtc				= SYSUTCDATETIME()
                		,cons.ConsentGrantedMessageId		= @MessageId
                		,cons.ConsentRejectedUtc			= NULL
                		,cons.ConsentRejectedReason			= NULL
                	FROM	dbo.UserConsentStatus	cons 
                WHERE	cons.UserId					= @UserId 
                	AND cons.CommunicationChannel	= @Channel
            ", (SqlConnection)conn);

            cmd.Parameters.Add("@UserId", SqlDbType.BigInt).Value = userId;
            cmd.Parameters.Add("@Channel", SqlDbType.VarChar, 32).Value = channel;
            cmd.Parameters.Add("@MessageId", SqlDbType.NVarChar, 128).Value = consentGrantedMessageId;

            await cmd.ExecuteNonQueryAsync();
        }

        private static byte[] CombineCode(byte[] salt, byte[] codeBytes)
        {
            var has = new byte[salt.Length + codeBytes.Length];
            Buffer.BlockCopy(salt, 0, has, 0, salt.Length);
            Buffer.BlockCopy(codeBytes, 0, has, salt.Length, codeBytes.Length);
            return has;
        }
    }
}
