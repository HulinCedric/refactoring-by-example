using System.Collections.Generic;
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

    public Either<UnknownProduct, Order> Run(List<CreateOrderItem> items)
        => CreateOrder(_productCatalog, items)
            .Map(order => _orderRepository.Save(order));
}