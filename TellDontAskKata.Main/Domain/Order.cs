using System.Collections.Generic;

namespace TellDontAskKata.Main.Domain;

public class Order
{
    public Order()
    {
    }

    private Order(OrderStatus status, IList<OrderItem> items, string currency, decimal total, decimal tax)
    {
        Status = status;
        Items = items;
        Currency = currency;
        Total = total;
        Tax = tax;
    }

    public string Currency { get; }
    public int Id { get; init; }
    public IList<OrderItem> Items { get; }
    public OrderStatus Status { get; set; }
    public decimal Tax { get; set; }

    public decimal Total { get; set; }

    public static Order CreateOrder()
        => new(
            status: OrderStatus.Created,
            items: new List<OrderItem>(),
            currency: "EUR",
            total: 0m,
            tax: 0m);
}