using CsvHelper.Configuration;

namespace ZadanieRekrutacyjne.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string EAN { get; set; }
        public string ProducerName { get; set; }
        public string Category { get; set; }
        public bool IsWire { get; set; }
        public bool Available { get; set; }
        public bool IsVendor { get; set; }
        public string DefaultImage { get; set; }
    }
    public sealed class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Map(m => m.Id).Name("ID");
            Map(m => m.Name).Name("name");
            Map(m => m.Sku).Name("sku");
            Map(m => m.EAN).Name("EAN");
            Map(m => m.ProducerName).Name("producer_name");
            Map(m => m.Category).Name("category");
            Map(m => m.IsWire).Name("is_wire");
            Map(m => m.Available).Name("available");
            Map(m => m.IsVendor).Name("is_vendor");
            Map(m => m.DefaultImage).Name("default_image");
        }
    }
}
