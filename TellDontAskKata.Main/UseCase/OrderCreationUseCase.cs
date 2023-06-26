﻿using System.Collections.Generic;
using TellDontAskKata.Main.Commands;
using TellDontAskKata.Main.Domain;
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
            var product = _productCatalog.GetByName(item.Name);
            if (product == null)
                throw new UnknownProductException();
            
            var orderItem = CreateOrderItem(item, product);

            order.AddItem(orderItem);
        }

        _orderRepository.Save(order);
    }
}