using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WAChatFlow.Server.Repositories.DataBase;
using WAChatFlow.Shared.Common;
using WAChatFlow.Shared.Models.Users;
using static WAChatFlow.Shared.Models.Dtos.UsersDto;

namespace WAChatFlow.Server.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DatabaseConnectionFactory _dbConnection;

        public UsersRepository(DatabaseConnectionFactory connection)
        {
            _dbConnection = connection;
        }

        public async Task<IEnumerable<UserStatusRow>> GetUserStatusCatalogAsync(CancellationToken ct = default)
        {
            try
            {
                using var conn = await _dbConnection.CreateOpenConnectionAsync(ct);

                var rows = await conn.QueryAsync<UserStatusRow>(
                    new CommandDefinition(
                        "dbo.spConsentStatusCatalog",
                        commandType: CommandType.StoredProcedure,
                        cancellationToken: ct));

                return rows;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos al obtener el catálogo de usuarios.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado en GetUserStatusCatalogAsync.", ex);
            }
        }

        public async Task<NotificationUser> GetByIdAsync(long userId, CancellationToken ct = default)
        {
            var conn = await _dbConnection.CreateOpenConnectionAsync(ct);

            var sql = @"SELECT tbl.UserId, tbl.PhoneNumber, tbl.FullName, tbl.CreatedUtc FROM dbo.NotificationUser tbl WHERE tbl.UserId = @UserId";

            return await conn.QuerySingleOrDefaultAsync<NotificationUser>(
                new CommandDefinition(sql, new { UserId = userId }, commandType: CommandType.Text, cancellationToken: ct));
        }

        public async Task<NotificationUser> GetByPhoneAsync(string phoneNumber, CancellationToken ct = default)
        {
            var conn = await _dbConnection.CreateOpenConnectionAsync(ct);

            var sql = @"SELECT tbl.UserId, tbl.PhoneNumber, tbl.FullName, tbl.CreatedUtc FROM dbo.NotificationUser tbl WHERE tbl.PhoneNumber = @PhoneNumber";

            return await conn.QuerySingleOrDefaultAsync<NotificationUser>(
                new CommandDefinition(sql, new { PhoneNumber = phoneNumber }, commandType: CommandType.Text, cancellationToken: ct));
        }

        public async Task<long> UpsertByPhoneAsync(string phoneNumber, string fullName, CancellationToken ct = default)
        {
            var conn = await _dbConnection.CreateOpenConnectionAsync(ct);

            var sql = @"
                        DECLARE	 @UserId			BIGINT = 0 

                        UPDATE tbl SET tbl.FullName = COALESCE(NULLIF(@FullName, ''), tbl.FullName) FROM dbo.NotificationUser tbl WHERE tbl.PhoneNumber = @PhoneNumber 
                        
                        IF @@ROWCOUNT > 0
                        	BEGIN
                        		SELECT 
                        				 @UserId		= tbl.UserId
                        			FROM	dbo.NotificationUser	tbl 
                        	    WHERE	tbl.PhoneNumber = @PhoneNumber
                        	END
                        ELSE
                        	BEGIN
                        	    BEGIN TRY
                        	        INSERT INTO dbo.NotificationUser	(PhoneNumber, FullName) 
                        										VALUES	(@PhoneNumber, NULLIF(@FullName, ''))
                        	        SET @UserId = CAST(SCOPE_IDENTITY() AS BIGINT) 
                        	    END TRY
                        	    BEGIN CATCH
                        	        IF ERROR_NUMBER() IN (2601, 2627)
                        				BEGIN
                        				    SELECT 
                        							 @UserId		= tbl.UserId 
                        						FROM	dbo.NotificationUser	tbl 
                        					WHERE	tbl.PhoneNumber = @PhoneNumber
                        				END
                        			ELSE
                        				THROW 
                        	    END CATCH
                        	END
                        
                        SELECT @UserId";

            return await conn.ExecuteScalarAsync<long>(
                new CommandDefinition(sql, new { phoneNumber, fullName }, cancellationToken: ct));
        }
    }
}
