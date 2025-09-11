using Microsoft.Data.SqlClient;

namespace WAChatFlow.Server.Repositories.DataBase
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AarcoWA")
                ?? throw new InvalidOperationException("Connection string 'AarcoWA' not found.");
        }

        public SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
