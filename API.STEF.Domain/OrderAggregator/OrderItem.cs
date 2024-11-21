using API.STEF.Domain.ProductAggregator;
using API.STEF.Domain.Shared.Entities;

namespace API.STEF.Domain.OrderAggregator
{
    public class OrderItem:Entity
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

        protected OrderItem()
        {
            
        }

        internal static OrderItem CreateNew(int orderId, int productId, int quantity)
        {
            return new()
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity
            };
        }
    }
}
