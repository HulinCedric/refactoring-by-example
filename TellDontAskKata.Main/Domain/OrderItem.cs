using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain;

public class OrderItem
{
    private OrderItem(Product product, int quantity)
    {
        var unitaryTax = product.GetUnitaryTax();
        var unitaryTaxedAmount = product.GetUnitaryTaxedAmount();

        Product = product;
        Quantity = quantity;
        Tax = (unitaryTax * quantity).Round();
        TaxedAmount = (unitaryTaxedAmount * quantity).Round();
    }

    public Product Product { get; }
    public int Quantity { get; }
    public decimal Tax { get; }
    public decimal TaxedAmount { get; }

    public static OrderItem CreateOrderItem(CreateOrderItem item, Product product)
        => new(
            product: product,
            quantity: item.Quantity);
}