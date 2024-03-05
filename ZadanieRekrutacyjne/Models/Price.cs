using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ZadanieRekrutacyjne.Models
{
    public class Price
    {
        public string Sku { get; set; }
        public decimal? VatRate { get; set; }
        public decimal? ValueNetAfterUnitDiscount { get; set; }
    }
    public sealed class PriceMap : ClassMap<Price>
    {
        public PriceMap()
        {
            Map(m => m.Sku).Index(1);
            Map(m => m.VatRate).Index(4).TypeConverter<DecimalCustomConverter>();
            Map(m => m.ValueNetAfterUnitDiscount).Index(5);
        }
    }
    public class DecimalCustomConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return Decimal.TryParse(text, out decimal result) ? result : 0;
        }
    }
}
