using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.UseCase;
using TellDontAskKata.Tests.Doubles;
using Xunit;
using static TellDontAskKata.Tests.Builders.OrderTestBuilder;

namespace TellDontAskKata.Tests.UseCase
{
    public class OrderApprovalUseCaseTest
    {
        private readonly TestOrderRepository _orderRepository;
        private readonly OrderApprovalUseCase _useCase;

        public OrderApprovalUseCaseTest()
        {
            _orderRepository = new TestOrderRepository();
            _useCase = new OrderApprovalUseCase(_orderRepository);
        }


        [Fact]
        public void ApprovedExistingOrder()
        {
            var initialOrder = Order().WithStatus(OrderStatus.Created).Build();
           
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = true
            };

            _useCase.Run(request);

            var savedOrder = _orderRepository.GetSavedOrder();
            Assert.Equal(OrderStatus.Approved, savedOrder.Status);
        }

        [Fact]
        public void RejectedExistingOrder()
        {
            // use builder
            var initialOrder = Order().WithStatus(OrderStatus.Created).Build();
                
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = false
            };

            _useCase.Run(request);

            var savedOrder = _orderRepository.GetSavedOrder();
            Assert.Equal(OrderStatus.Rejected, savedOrder.Status);
        }


        [Fact]
        public void CannotApproveRejectedOrder()
        {
            var initialOrder = Order().WithStatus(OrderStatus.Rejected).Build();

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = true
            };


            var actionToTest = () => _useCase.Run(request);
      
            Assert.Throws<RejectedOrderCannotBeApprovedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
        }

        [Fact]
        public void CannotRejectApprovedOrder()
        {
            var initialOrder = Order().WithStatus(OrderStatus.Approved).Build();

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = false
            };


            var actionToTest = () => _useCase.Run(request);
            
            Assert.Throws<ApprovedOrderCannotBeRejectedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
        }

        [Fact]
        public void ShippedOrdersCannotBeRejected()
        {
            var initialOrder = Order().WithStatus(OrderStatus.Shipped).Build();

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = false
            };


            var actionToTest = () => _useCase.Run(request);

            Assert.Throws<ShippedOrdersCannotBeChangedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
        }

    }
}
