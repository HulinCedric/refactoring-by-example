using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Repository;

namespace TellDontAskKata.Tests.Doubles;

public class TestOrderRepository : IOrderRepository
{
    private Order _insertedOrder;
    private readonly IList<Order> _orders = new List<Order>();

    public Order Save(Order order)
        => _insertedOrder = order;

    public Order GetById(int orderId)
        => _orders.FirstOrDefault(o => o.Id == orderId);


    public Order GetSavedOrder()
        => _insertedOrder;

    public void AddOrder(Order order)
        => _orders.Add(order);
}