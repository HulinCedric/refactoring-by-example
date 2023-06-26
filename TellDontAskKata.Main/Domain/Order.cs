using System.Collections.Generic;
using System.Linq;
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

    public Order AddItem(OrderItem orderItem)
    {
        Items.Add(orderItem);
        Total += orderItem.TaxedAmount;
        Tax += orderItem.Tax;

        return this;
    }

    public static Order CreateOrder(IProductCatalog productCatalog, List<CreateOrderItem> itemRequests)
        => itemRequests
            .Select(itemRequest => NewOrderItem(productCatalog, itemRequest))
            .Aggregate(NewOrder(), (order, item) => order.AddItem(item));
}