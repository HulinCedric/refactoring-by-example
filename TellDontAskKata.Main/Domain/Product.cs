namespace TellDontAskKata.Main.Domain
{
    public class Product
    {
        public string Name { get; init; }
        public decimal Price { get; init; }
        public Category Category { get; init; }

        public decimal GetUnitaryTaxedAmount()
            => (Price + GetUnitaryTax()).Round();

        public decimal GetUnitaryTax()
            => (Price / 100m * Category.TaxPercentage).Round();
    }
}