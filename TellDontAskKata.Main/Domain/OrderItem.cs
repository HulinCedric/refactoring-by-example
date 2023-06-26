using System;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain;

public class OrderItem
{
    private OrderItem(Product product, int quantity)
    {
        var unitaryTax = Round(product.Price / 100m * product.Category.TaxPercentage);
        var unitaryTaxedAmount = Round(product.Price + unitaryTax);

        Product = product;
        Quantity = quantity;
        Tax = Round(unitaryTax * quantity);
        TaxedAmount = Round(unitaryTaxedAmount * quantity);
    }

    public Product Product { get; }
    public int Quantity { get; }
    public decimal Tax { get; }
    public decimal TaxedAmount { get; }

    public static OrderItem CreateOrderItem(CreateOrderItem item, Product product)
        => new(
            product: product,
            quantity: item.Quantity);

    private static decimal Round(decimal amount)
        => decimal.Round(amount, 2, MidpointRounding.ToPositiveInfinity);
}