using System.Collections.Generic;
using TellDontAskKata.Main.Commands;
using TellDontAskKata.Main.Repository;
using static TellDontAskKata.Main.Domain.OrderItem;

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
    public decimal Tax { get; private set; }
    public decimal Total { get; private set; }

    public static Order NewOrder()
        => new();

    public void AddItem(OrderItem orderItem)
    {
        Items.Add(orderItem);
        Total += orderItem.TaxedAmount;
        Tax += orderItem.Tax;
    }

    public static Order CreateOrder(IProductCatalog productCatalog, List<CreateOrderItem> items)
    {
        var order = NewOrder();

        foreach (var item in items)
            order.AddItem(NewOrderItem(productCatalog, item));

        return order;
    }
}