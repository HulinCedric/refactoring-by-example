using System.Collections.Generic;

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

        public decimal Total { get; set; }
        public string Currency { get; init; }
        public IList<OrderItem> Items { get; init; }
        public decimal Tax { get; set; }
        public OrderStatus Status { get; set; }
        public int Id { get; init; }

        public void Add(OrderItem orderItem)
        {
            Items.Add(orderItem);
            Total += orderItem.TaxedAmount;
            Tax += orderItem.Tax;
        }
    }
}