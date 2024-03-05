using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ZadanieRekrutacyjne.DAL.Models;

namespace ZadanieRekrutacyjne.DAL.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration configuratio;

        public ProductRepository(IConfiguration configuratio)
        {
            this.configuratio = configuratio;
        }
        public void AddProduct(List<Product> toSave)
        {
            using (var connection = new SqlConnection(configuratio.GetConnectionString("connDB") + "Database=rekrutacjaintegracja"))
            {
                var sql = "SET IDENTITY_INSERT Product ON INSERT INTO Product (Id, Sku, Name, EAN, ProducerName, Category, IsWire, Available, IsVendor, DefaultImage) " +
                    "VALUES (@Id, @Sku, @Name, @EAN, @ProducerName, @Category, @IsWire, @Available, @IsVendor, @DefaultImage) SET IDENTITY_INSERT Product OFF";

                var rowsAffected = connection.Execute(sql, toSave);
            }
        }
        public async Task<Product?> GetProduct(string sku)
        {
            try
            {
                using (var connection = new SqlConnection(configuratio.GetConnectionString("connDB") + "Database=rekrutacjaintegracja"))
                {
                    return await connection.QuerySingleAsync<Product>("SELECT * FROM Product WHERE Sku = @sku",
                        param: new { sku });
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
