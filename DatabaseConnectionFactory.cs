using Microsoft.Data.SqlClient;
using System.Data;

namespace WAChatFlow.Server.Repositories.DataBase
{
    public class DatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public DatabaseConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AarcoWA")
                ?? throw new InvalidOperationException("Connection string 'AarcoWA' not found.");
        }

        public async Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken ct = default)
        {
            var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(ct);
            return conn;
        }
    }
}