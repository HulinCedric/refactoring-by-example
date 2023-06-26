using System.Collections.Generic;
using TellDontAskKata.Main.Repository;
using static TellDontAskKata.Main.Domain.Order;
using static TellDontAskKata.Main.Domain.OrderItem;

namespace TellDontAskKata.Main.UseCase;

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

    public void Run(List<CreateOrderItem> items)
    {
        var order = CreateOrder();

        foreach (var item in items)
        {
            var orderItem = CreateOrderItem(_productCatalog, item);

            order.Items.Add(orderItem);
            order.Total += orderItem.TaxedAmount;
            order.Tax += orderItem.Tax;
        }

        _orderRepository.Save(order);
    }
}