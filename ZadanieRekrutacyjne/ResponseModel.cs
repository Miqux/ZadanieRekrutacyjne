namespace ZadanieRekrutacyjne
{
    public class ResponseModel
    {
        public string Name { get; set; }
        public string EAN { get; set; }
        public string ProducerName { get; set; }
        public string Category { get; set; }
        public string ImageURL { get; set; }
        public decimal QuantityWarehouse { get; set; }
        public string Unit { get; set; }
        public decimal? NetPrice { get; set; }
        public decimal? ShippingCost { get; set; }

    }
}
