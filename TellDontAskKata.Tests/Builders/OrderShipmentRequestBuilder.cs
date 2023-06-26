using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Tests.Builders;

public class OrderShipmentRequestBuilder
{
    private readonly int _orderId;

    private OrderShipmentRequestBuilder()
        => _orderId = OrderDefaults.Id;

    public static OrderShipmentRequestBuilder OrderShipmentRequest()
        => new();

    public OrderShipmentRequest Build()
        => new()
        {
            OrderId = _orderId
        };
}