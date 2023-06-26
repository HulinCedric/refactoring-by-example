using TellDontAskKata.Main.Domain;

namespace TellDontAskKata.Tests.Builders;

public class OrderTestBuilder
{
    private int _orderId;
    private OrderStatus _orderStatus;

    private OrderTestBuilder()
    {
        _orderId = 1;
        _orderStatus = OrderStatus.Created;
    }

    public static OrderTestBuilder Order()
        => new();

    public OrderTestBuilder WithStatus(OrderStatus orderStatus)
    {
        _orderStatus = orderStatus;

        return this;
    }

    public OrderTestBuilder WithId(int orderId)
    {
        _orderId = orderId;

        return this;
    }

    public Order Build()
        => new(id: _orderId, status: _orderStatus);
}