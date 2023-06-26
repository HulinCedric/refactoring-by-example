using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using TellDontAskKata.Main.Commands;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.Service;
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
    public OrderStatus Status { get; private set; }
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

    public static Either<UnknownProducts, Order> CreateOrder(
        IProductCatalog productCatalog,
        List<CreateOrderItem> itemRequests)
    {
        var orderItems = ToOrderItems(productCatalog, itemRequests);
        return ContainFailure(orderItems) ?
                   ToFailure(orderItems) :
                   ToSuccess(orderItems);
    }

    private static Order ToSuccess(IEnumerable<Either<UnknownProduct, OrderItem>> orderItems)
        => orderItems.Rights()
            .Aggregate(NewOrder(), (order, item) => order.AddItem(item));

    private static Either<UnknownProduct, OrderItem>[] ToOrderItems(
        IProductCatalog productCatalog,
        IEnumerable<CreateOrderItem> itemRequests)
        => itemRequests
            .Select(itemRequest => NewOrderItem(productCatalog, itemRequest))
            .ToArray();

    private static UnknownProducts ToFailure(IEnumerable<Either<UnknownProduct, OrderItem>> orderItems)
        => new(orderItems.Lefts());

    private static bool ContainFailure(IEnumerable<Either<UnknownProduct, OrderItem>> orderItems)
        => orderItems.Lefts().Any();

    public void Approve(OrderApprovalRequest request)
    {
        if (Status == OrderStatus.Shipped)
            throw new ShippedOrdersCannotBeChangedException();

        if (request.Approved &&
            Status == OrderStatus.Rejected)
            throw new RejectedOrderCannotBeApprovedException();

        if (!request.Approved &&
            Status == OrderStatus.Approved)
            throw new ApprovedOrderCannotBeRejectedException();

        Status = request.Approved ? OrderStatus.Approved : OrderStatus.Rejected;
    }

    public void Ship(IShipmentService shipmentService)
    {
        if (Status == OrderStatus.Created ||
            Status == OrderStatus.Rejected)
            throw new OrderCannotBeShippedException();

        if (Status == OrderStatus.Shipped)
            throw new OrderCannotBeShippedTwiceException();

        shipmentService.Ship(this);

        Status = OrderStatus.Shipped;
    }
}