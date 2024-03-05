using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ZadanieRekrutacyjne.DAL
{
    public class DataContext
    {
        private string connectionString;

        private readonly IConfiguration configuration;

        public DataContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("connDB");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString + "Database=rekrutacjaintegracja");
        }

        public async Task Init()
        {
            await _initDatabase();
            await _initTables();
        }

        private async Task _initDatabase()
        {
            using var connection = new SqlConnection(connectionString + "Database=master;");
            var sql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'rekrutacjaintegracja') CREATE DATABASE [rekrutacjaintegracja];";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initTables()
        {
            using var connection = CreateConnection();
            await _initInventory();
            await _initProducts();
            await _initPrices();

            async Task _initInventory()
            {
                var sql = """
                IF OBJECT_ID('Inventory', 'U') IS NULL
                CREATE TABLE Inventory (
                    Id INT NOT NULL PRIMARY KEY IDENTITY,
                    Sku NVARCHAR(MAX),
                    Unit NVARCHAR(MAX),
                    ShippingCost DECIMAL(25,5),
                    Qty DECIMAL(25,8)
                );
                """;
                await connection.ExecuteAsync(sql);
            }

            async Task _initProducts()
            {
                var sql = """
                IF OBJECT_ID('Product', 'U') IS NULL
                CREATE TABLE Product (
                    Id INT NOT NULL PRIMARY KEY IDENTITY,
                    Sku NVARCHAR(MAX),
                    Name NVARCHAR(MAX),
                    EAN NVARCHAR(MAX),
                    ProducerName NVARCHAR(MAX),
                    Category NVARCHAR(MAX),
                    DefaultImage NVARCHAR(MAX),
                    IsWire BIT,
                    Available BIT,
                    IsVendor BIT
                );
                """;
                await connection.ExecuteAsync(sql);
            }

            async Task _initPrices()
            {
                var sql = """
                IF OBJECT_ID('Price', 'U') IS NULL
                CREATE TABLE Price (
                    Id INT NOT NULL PRIMARY KEY IDENTITY,
                    Sku NVARCHAR(MAX),
                    VatRate DECIMAL(7,4),
                    ValueNetAfterUnitDiscount DECIMAL(25,5)
                );
                """;
                await connection.ExecuteAsync(sql);
            }
        }
    }
}
