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
                var product = _productCatalog.GetByName(itemRequest.ProductName);

                if (product == null)
                {
                    throw new UnknownProductException();
                }

                var unitaryTax = product.GetUnitaryTax();
                var unitaryTaxedAmount = product.GetUnitaryTaxedAmount();
                var taxedAmount = (unitaryTaxedAmount * itemRequest.Quantity).Round();
                var taxAmount = (unitaryTax * itemRequest.Quantity).Round();

                var orderItem = new OrderItem
                {
                    Product = product,
                    Quantity = itemRequest.Quantity,
                    Tax = taxAmount,
                    TaxedAmount = taxedAmount
                };
                
                order.Items.Add(orderItem);
                order.Total += orderItem.TaxedAmount;
                order.Tax += orderItem.Tax;
            }

            _orderRepository.Save(order);
        }
    }
    
    public record ItemRequest(string ProductName, int Quantity);

}