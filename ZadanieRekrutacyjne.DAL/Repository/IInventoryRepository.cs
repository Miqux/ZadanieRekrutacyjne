using ZadanieRekrutacyjne.DAL.Models;

namespace ZadanieRekrutacyjne.DAL.Repository
{
    public interface IInventoryRepository
    {
        void AddInvetory(List<Inventory> toSave);
        Task<Inventory?> GetInventory(string sku);
    }
}
