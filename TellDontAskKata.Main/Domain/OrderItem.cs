using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public class OrderItem
    {
        private OrderItem(Product product, int quantity)
        {
            var unitaryTax = product.GetUnitaryTax();
            var unitaryTaxedAmount = product.GetUnitaryTaxedAmount();
            
            Product = product;
            Quantity = quantity;
            Tax = GetTaxAmount(unitaryTax, quantity);
            TaxedAmount = GetTaxedAmount(unitaryTaxedAmount, quantity);
        }

        public static OrderItem New(Product product, int quantity)
        {
            return new OrderItem(product, quantity);
        }

        public Product Product { get; }
        public int Quantity { get; }
        public decimal TaxedAmount { get; }
        public decimal Tax { get; }

        private static decimal GetTaxAmount(decimal unitaryTax, int quantity)
            => (unitaryTax * quantity).Round();

        private static decimal GetTaxedAmount(decimal unitaryTaxedAmount, int quantity)
            => (unitaryTaxedAmount * quantity).Round();

        public static OrderItem CreateItem(IProductCatalog productCatalog, ItemRequest itemRequest)
        {
            var product = productCatalog.GetByName(itemRequest.ProductName);

            if (product == null)
            {
                throw new UnknownProductException();
            }

            return New(product: product, quantity: itemRequest.Quantity);
        }
    }
}
