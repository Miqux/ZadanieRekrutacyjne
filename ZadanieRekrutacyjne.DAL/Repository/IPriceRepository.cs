using ZadanieRekrutacyjne.DAL.Models;

namespace ZadanieRekrutacyjne.DAL.Repository
{
    public interface IPriceRepository
    {
        void AddPrice(List<Price> toSave);
        Task<Price?> GetPrice(string sku);
    }
}
