using System.Collections.Generic;
using TellDontAskKata.Main.Commands;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.UseCase;

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

    public static Order New()
        => new();

    public void AddItem(OrderItem orderItem)
    {
        Items.Add(orderItem);
        Total += orderItem.TaxedAmount;
        Tax += orderItem.Tax;
    }

    public static Order CreateOrder(IProductCatalog productCatalog, List<CreateOrderItem> items)
    {
        var order = Order.New();

        foreach (var item in items)
        {
            var product = productCatalog.GetByName(item.Name);
            if (product == null)
                throw new UnknownProductException();

            var orderItem = OrderItem.CreateOrderItem(item, product);

            order.AddItem(orderItem);
        }

        return order;
    }
}