using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.UseCase;
using TellDontAskKata.Tests.Doubles;
using Xunit;
using static TellDontAskKata.Tests.Builders.OrderTestBuilder;

namespace TellDontAskKata.Tests.UseCase;

public class OrderShipmentUseCaseTest
{
    private readonly TestOrderRepository _orderRepository;
    private readonly TestShipmentService _shipmentService;
    private readonly OrderShipmentUseCase _useCase;

    public OrderShipmentUseCaseTest()
    {
        _orderRepository = new TestOrderRepository();
        _shipmentService = new TestShipmentService();
        _useCase = new OrderShipmentUseCase(_orderRepository, _shipmentService);
    }


    [Fact]
    public void ShipApprovedOrder()
    {
        var initialOrder = Order().WithId(1).WithStatus(OrderStatus.Approved).Build();

        _orderRepository.AddOrder(initialOrder);

        var request = new OrderShipmentRequest
        {
            OrderId = 1
        };

        _useCase.Run(request);

        Assert.Equal(OrderStatus.Shipped, _orderRepository.GetSavedOrder().Status);
        Assert.Same(initialOrder, _shipmentService.GetShippedOrder());
    }

    [Fact]
    public void CreatedOrdersCannotBeShipped()
    {
        var initialOrder = Order().WithId(1).WithStatus(OrderStatus.Created).Build();

        _orderRepository.AddOrder(initialOrder);

        var request = new OrderShipmentRequest
        {
            OrderId = 1
        };

        var actionToTest = () => _useCase.Run(request);

        Assert.Throws<OrderCannotBeShippedException>(actionToTest);
        Assert.Null(_orderRepository.GetSavedOrder());
        Assert.Null(_shipmentService.GetShippedOrder());
    }

    [Fact]
    public void RejectedOrdersCannotBeShipped()
    {
        var initialOrder = Order().WithId(1).WithStatus(OrderStatus.Rejected).Build();

        _orderRepository.AddOrder(initialOrder);

        var request = new OrderShipmentRequest
        {
            OrderId = 1
        };

        var actionToTest = () => _useCase.Run(request);

        Assert.Throws<OrderCannotBeShippedException>(actionToTest);
        Assert.Null(_orderRepository.GetSavedOrder());
        Assert.Null(_shipmentService.GetShippedOrder());
    }

    [Fact]
    public void ShippedOrdersCannotBeShippedAgain()
    {
        var initialOrder = Order().WithId(1).WithStatus(OrderStatus.Shipped).Build();

        _orderRepository.AddOrder(initialOrder);

        var request = new OrderShipmentRequest
        {
            OrderId = 1
        };

        var actionToTest = () => _useCase.Run(request);

        Assert.Throws<OrderCannotBeShippedTwiceException>(actionToTest);
        Assert.Null(_orderRepository.GetSavedOrder());
        Assert.Null(_shipmentService.GetShippedOrder());
    }
}