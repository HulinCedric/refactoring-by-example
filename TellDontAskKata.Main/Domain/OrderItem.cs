using System;
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
        Tax = Round(unitaryTax * quantity);
        TaxedAmount = Round(unitaryTaxedAmount * quantity);
    }

    public Product Product { get; }
    public int Quantity { get; }
    public decimal Tax { get; }
    public decimal TaxedAmount { get; }

    private static decimal GetUnitaryTaxedAmount(Product product, decimal unitaryTax)
        => Round(product.Price + unitaryTax);

    private static decimal GetUnitaryTax(Product product)
        => Round(product.Price / 100m * product.Category.TaxPercentage);

    public static OrderItem CreateOrderItem(CreateOrderItem item, Product product)
        => new(
            product: product,
            quantity: item.Quantity);

    private static decimal Round(decimal amount)
        => decimal.Round(amount, 2, MidpointRounding.ToPositiveInfinity);
}