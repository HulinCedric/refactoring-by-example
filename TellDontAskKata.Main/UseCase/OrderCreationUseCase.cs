using System.Collections.Generic;
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

        public void Run(Dictionary<string, int> items)
        {
            var order = new Order
            {
                Status = Created,
                Items = new List<OrderItem>(),
                Currency = "EUR",
                Total = 0m,
                Tax = 0m
            };

            foreach (var itemRequest in items)
            {
                var product = _productCatalog.GetByName(itemRequest.Key);

                if (product == null)
                {
                    throw new UnknownProductException();
                }

                var unitaryTax = product.GetUnitaryTax();
                var unitaryTaxedAmount = product.GetUnitaryTaxedAmount();
                var taxedAmount = (unitaryTaxedAmount * itemRequest.Value).Round();
                var taxAmount = (unitaryTax * itemRequest.Value).Round();

                var orderItem = new OrderItem
                {
                    Product = product,
                    Quantity = itemRequest.Value,
                    Tax = taxAmount,
                    TaxedAmount = taxedAmount
                };
                order.Items.Add(orderItem);
                order.Total += taxedAmount;
                order.Tax += taxAmount;
            }

            _orderRepository.Save(order);
        }
    }
}