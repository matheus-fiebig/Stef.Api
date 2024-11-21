using API.STEF.Domain.Shared.Entities;
using API.STEF.Domain.Shared.Models;
using System.Collections.Generic;

namespace API.STEF.Domain.OrderAggregator
{
    public class Order : Entity
    {
        public CustomerVO Customer { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public bool Paid { get; private set; }

        public virtual List<OrderItem> OrderItems { get; set; }

        protected Order()
        {
            
        }

        public static Order CreateNew(
            CustomerVO customer, 
            List<OrderItemDto> items
         )
        {
            var order = new Order()
            {
                Customer = customer,
                CreatedAt = DateTime.Now,
                OrderItems = items.Select(x => OrderItem.CreateNew(0, x.ProductId, x.Quantity)).ToList()
            };
            return order;
        }

        public void Update(
            CustomerVO customer,
            List<OrderItemDto> items,
            bool paid
        )
        {
            Paid = paid; 
            Customer = customer;

            OrderItems = OrderItems
                .Where(orderItem => items.Any(item => item.ProductId == orderItem.ProductId))
                .ToList();

            OrderItems.ForEach(orderItem =>
            {
                var matchingItem = items.FirstOrDefault(item => item.ProductId == orderItem.ProductId);
                if (matchingItem != null)
                {
                    orderItem.Quantity = matchingItem.Quantity;
                }
            });

            var newOrderItems = items
                .Where(item => OrderItems.All(orderItem => orderItem.ProductId != item.ProductId))
                .Select(item => OrderItem.CreateNew(Id, item.ProductId, item.Quantity));

            OrderItems.AddRange(newOrderItems);
        }

        public void PayOrder()
        {
            Paid = true;
        }

        public bool CanChangeOrder()
        {
            return Paid;
        }
    }
}
