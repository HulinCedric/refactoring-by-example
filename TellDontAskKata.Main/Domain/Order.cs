using System.Collections.Generic;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        public Order()
        {
            Status = OrderStatus.Created;
            Items = new List<OrderItem>();
            Currency = "EUR";
            Total = 0m;
            Tax = 0;
        }

        public static Order NewOrder()
            => new();

        public decimal Total { get; private set; }
        public string Currency { get; }
        public IList<OrderItem> Items { get; }
        public decimal Tax { get; private set; }
        public OrderStatus Status { get; set; }
        public int Id { get; init; }

        public void Add(OrderItem orderItem)
        {
            Items.Add(orderItem);
            Total += orderItem.TaxedAmount;
            Tax += orderItem.Tax;
        }

        public static Order CreateOrder(IProductCatalog productCatalog, List<ItemRequest> items)
        {
            var order = NewOrder();

            foreach (var itemRequest in items)
            {
                var orderItem = OrderItem.GetOrderItem(productCatalog, itemRequest);

                order.Add(orderItem);
            }

            return order;
        }
    }
}