using TellDontAskKata.Main.Domain;

namespace TellDontAskKata.Tests.Builders;

public class OrderTestBuilder
{
    private OrderStatus _orderStatus;

    private OrderTestBuilder()
        => _orderStatus = OrderStatus.Created;

    public static OrderTestBuilder Order()
        => new();

    public OrderTestBuilder WithStatus(OrderStatus orderStatus)
    {
        _orderStatus = orderStatus;

        return this;
    }

    public Order Build()
        => new()
        {
            Status = _orderStatus,
            Id = 1
        };
}