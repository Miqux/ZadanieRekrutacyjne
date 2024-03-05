using CsvHelper.Configuration;

namespace ZadanieRekrutacyjne.Models
{
    public class Inventory
    {
        public string Sku { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public string Shipping { get; set; }
        public decimal? ShippingCost { get; set; }
    }
    public sealed class InventoryMap : ClassMap<Inventory>
    {
        public InventoryMap()
        {
            Map(m => m.Sku).Name("sku");
            Map(m => m.Unit).Name("unit");
            Map(m => m.Qty).Name("qty");
            Map(m => m.Shipping).Name("shipping");
            Map(m => m.ShippingCost).Name("shipping_cost");
        }
    }
}
