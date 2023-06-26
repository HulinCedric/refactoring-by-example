using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Main.Commands;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Tests.Doubles
{
    public class InMemoryProductCatalog : IProductCatalog
    {
        private readonly IList<Product> _products;

        public InMemoryProductCatalog(IList<Product> products)
        {
            _products = products;
        }

        public Product GetByName(string name)
        {
            return _products.FirstOrDefault(p => p.Name == name);
        }

        public Order CreateOrder(List<CreateOrderItem> items)
        {
            var order = Order.New();

            foreach (var item in items)
            {
                var product = GetByName(item.Name);
                if (product == null)
                    throw new UnknownProductException();

                var orderItem = OrderItem.CreateOrderItem(item, product);

                order.AddItem(orderItem);
            }

            return order;
        }
    }
}
