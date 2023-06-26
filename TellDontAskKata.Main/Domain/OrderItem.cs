using System;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain;

public class OrderItem
{
    private OrderItem(Product product, int quantity, decimal tax, decimal taxedAmount)
    {
        Product = product;
        Quantity = quantity;
        Tax = tax;
        TaxedAmount = taxedAmount;
    }

    public Product Product { get; }
    public int Quantity { get; }
    public decimal Tax { get; }
    public decimal TaxedAmount { get; }

    public static OrderItem CreateOrderItem(CreateOrderItem item, Product product)
    {
      

        var unitaryTax = Round(product.Price / 100m * product.Category.TaxPercentage);
        var unitaryTaxedAmount = Round(product.Price + unitaryTax);
        var taxedAmount = Round(unitaryTaxedAmount * item.Quantity);
        var taxAmount = Round(unitaryTax * item.Quantity);

        return new OrderItem(
            product: product,
            quantity: item.Quantity,
            tax: taxAmount,
            taxedAmount: taxedAmount);
    }

    private static decimal Round(decimal amount)
        => decimal.Round(amount, 2, MidpointRounding.ToPositiveInfinity);
}