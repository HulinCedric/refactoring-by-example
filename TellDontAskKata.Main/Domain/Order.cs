using System.Collections.Generic;

namespace TellDontAskKata.Main.Domain;

public class Order
{
    public Order()
    {
        Status = OrderStatus.Created;
        Items = new List<OrderItem>();
        Currency = "EUR";
        Total = 0m;
        Tax = 0m;
    }

    public string Currency { get; }
    public int Id { get; init; }
    public IList<OrderItem> Items { get; }
    public OrderStatus Status { get; set; }
    public decimal Tax { get; set; }

    public decimal Total { get; set; }

    public static Order CreateOrder()
        => new();
}