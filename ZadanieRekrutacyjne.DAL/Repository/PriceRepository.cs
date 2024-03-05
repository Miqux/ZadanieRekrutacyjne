using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ZadanieRekrutacyjne.DAL.Models;

namespace ZadanieRekrutacyjne.DAL.Repository
{
    public class PriceRepository : IPriceRepository
    {
        private readonly IConfiguration configuratio;

        public PriceRepository(IConfiguration configuratio)
        {
            this.configuratio = configuratio;
        }

        public void AddPrice(List<Price> toSave)
        {
            using (var connection = new SqlConnection(configuratio.GetConnectionString("connDB") + "Database=rekrutacjaintegracja"))
            {
                var sql = "INSERT INTO Price (Sku, VatRate, ValueNetAfterUnitDiscount) " +
                    "VALUES (@Sku, @VatRate, @ValueNetAfterUnitDiscount)";

                var rowsAffected = connection.Execute(sql, toSave);
            }
        }
        public async Task<Price?> GetPrice(string sku)
        {
            try
            {
                using (var connection = new SqlConnection(configuratio.GetConnectionString("connDB") + "Database=rekrutacjaintegracja"))
                {
                    return await connection.QuerySingleAsync<Price>("SELECT * FROM Price WHERE Sku = @sku",
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
