using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ZadanieRekrutacyjne.DAL
{
    public class DataContext
    {
        static string databaseName = "rekrutacjaintegracja";
        static string connectionString = "Server=localhost;Database=" + databaseName + ";Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
        static string connectionStringMaster = "Server=localhost;Database=master;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        public DataContext()
        {

        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        public async Task Init()
        {
            await _initDatabase();
            await _initTables();
        }

        private async Task _initDatabase()
        {
            using var connection = new SqlConnection(connectionStringMaster);
            var sql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '" + databaseName + "') CREATE DATABASE [" + databaseName + "];";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initTables()
        {
            using var connection = CreateConnection();
            await _initUsers();

            async Task _initUsers()
            {
                var sql = """
                IF OBJECT_ID('Inventory', 'U') IS NULL
                CREATE TABLE Inventory (
                    Id INT NOT NULL PRIMARY KEY IDENTITY,
                    Sku NVARCHAR(MAX),
                    Unit NVARCHAR(MAX),
                    Shipping datetime2(7)
                );
                """;
                await connection.ExecuteAsync(sql);
            }
        }
    }
}
