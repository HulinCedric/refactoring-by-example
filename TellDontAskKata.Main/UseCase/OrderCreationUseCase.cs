using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Repository;
using static TellDontAskKata.Main.Domain.OrderStatus;

namespace TellDontAskKata.Main.UseCase
{
    public static class DecimalExtensions
    {
        public static decimal Round(this decimal amount)
        {
            return decimal.Round(amount, 2, System.MidpointRounding.ToPositiveInfinity);
        }
    }

    public class OrderCreationUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductCatalog _productCatalog;

        public OrderCreationUseCase(
            IOrderRepository orderRepository,
            IProductCatalog productCatalog)
        {
            _orderRepository = orderRepository;
            _productCatalog = productCatalog;
        }

        public void Run(List<ItemRequest> items)
        {
            var order = Order.NewOrder();

            foreach (var itemRequest in items)
            {
                var orderItem = GetOrderItem(_productCatalog, itemRequest);

                order.Add(orderItem);
            }

            _orderRepository.Save(order);
        }

        private static OrderItem GetOrderItem(IProductCatalog productCatalog, ItemRequest itemRequest)
        {
            var product = productCatalog.GetByName(itemRequest.ProductName);

            if (product == null)
            {
                throw new UnknownProductException();
            }

            return OrderItem.New(product: product, quantity: itemRequest.Quantity);
        }
    }

    public record ItemRequest(string ProductName, int Quantity);
}