using System;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain;

public class OrderItem
{
    public Product Product { get; private init; }
    public int Quantity { get; private init; }
    public decimal Tax { get; private init; }
    public decimal TaxedAmount { get; private init; }

    public static OrderItem CreateOrderItem(IProductCatalog productCatalog, CreateOrderItem item)
    {
        var product = productCatalog.GetByName(item.Name);

        if (product == null)
            throw new UnknownProductException();

        var unitaryTax = Round(product.Price / 100m * product.Category.TaxPercentage);
        var unitaryTaxedAmount = Round(product.Price + unitaryTax);
        var taxedAmount = Round(unitaryTaxedAmount * item.Quantity);
        var taxAmount = Round(unitaryTax * item.Quantity);

        return new OrderItem
        {
            Product = product,
            Quantity = item.Quantity,
            Tax = taxAmount,
            TaxedAmount = taxedAmount
        };
    }

    private static decimal Round(decimal amount)
        => decimal.Round(amount, 2, MidpointRounding.ToPositiveInfinity);
}