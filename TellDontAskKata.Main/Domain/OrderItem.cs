using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain;

public class OrderItem
{
    private OrderItem(Product product, int quantity)
    {
        var unitaryTax = GetUnitaryTax(product);
        var unitaryTaxedAmount = GetUnitaryTaxedAmount(product, unitaryTax);

        Product = product;
        Quantity = quantity;
        Tax = (unitaryTax * quantity).Round();
        TaxedAmount = (unitaryTaxedAmount * quantity).Round();
    }

    public Product Product { get; }
    public int Quantity { get; }
    public decimal Tax { get; }
    public decimal TaxedAmount { get; }

    private static decimal GetUnitaryTaxedAmount(Product product, decimal unitaryTax)
        => (product.Price + unitaryTax).Round();

    private static decimal GetUnitaryTax(Product product)
        => (product.Price / 100m * product.Category.TaxPercentage).Round();

    public static OrderItem CreateOrderItem(CreateOrderItem item, Product product)
        => new(
            product: product,
            quantity: item.Quantity);
}