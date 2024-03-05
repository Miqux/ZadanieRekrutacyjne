using ZadanieRekrutacyjne.DAL.Models;

namespace ZadanieRekrutacyjne.DAL.Repository
{
    public interface IProductRepository
    {
        void AddProduct(List<Product> toSave);
        Task<Product?> GetProduct(string sku);
    }
}
