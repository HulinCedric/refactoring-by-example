﻿using System.Collections.Generic;
using LanguageExt;
using TellDontAskKata.Main.Commands;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Repository;
using static TellDontAskKata.Main.Domain.Order;

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

    public Either<UnknownProductException, Order> Run(List<CreateOrderItem> items)
    {
        Order order;
        try
        {
            order = CreateOrder(_productCatalog, items);
        }
        catch (UnknownProductException unknownProduct)
        {
            return unknownProduct;
        }

        _orderRepository.Save(order);

        return order;
    }
}