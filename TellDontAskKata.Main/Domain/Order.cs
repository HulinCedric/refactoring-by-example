using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Main.Commands;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.UseCase;
using static TellDontAskKata.Main.Domain.OrderItem;

namespace TellDontAskKata.Main.Domain;

public class Order
{
    private Order() : this(1, OrderStatus.Created)
    {
    }

    public Order(int id, OrderStatus status)
    {
        Id = id;
        Status = status;
        Items = new List<OrderItem>();
        Currency = "EUR";
        Total = 0m;
        Tax = 0m;
    }

    public string Currency { get; }
    public int Id { get; }
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

    public void Approve(OrderApprovalRequest request)
    {
        if (Status == OrderStatus.Shipped)
        {
            throw new ShippedOrdersCannotBeChangedException();
        }

        if (request.Approved &&
            Status == OrderStatus.Rejected)
        {
            throw new RejectedOrderCannotBeApprovedException();
        }

        if (!request.Approved &&
            Status == OrderStatus.Approved)
        {
            throw new ApprovedOrderCannotBeRejectedException();
        }

        Status = request.Approved ? OrderStatus.Approved : OrderStatus.Rejected;
    }
}