using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ZadanieRekrutacyjne.DAL.Models;

namespace ZadanieRekrutacyjne.DAL.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IConfiguration configuratio;

        public InventoryRepository(IConfiguration configuratio)
        {
            this.configuratio = configuratio;
        }

        public void AddInvetory(List<Inventory> toSave)
        {
            using (var connection = new SqlConnection(configuratio.GetConnectionString("connDB") + "Database=rekrutacjaintegracja"))
            {
                var sql = "INSERT INTO Inventory (Sku, Unit, ShippingCost, Qty) VALUES (@Sku, @Unit, @ShippingCost, @Qty)";

                var rowsAffected = connection.Execute(sql, toSave);
            }
        }
        public async Task<Inventory?> GetInventory(string sku)
        {
            try
            {
                using (var connection = new SqlConnection(configuratio.GetConnectionString("connDB") + "Database=rekrutacjaintegracja"))
                {
                    return await connection.QuerySingleAsync<Inventory>("SELECT * FROM Inventory WHERE Sku = @sku",
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
