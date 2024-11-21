using API.STEF.Domain.OrderAggregator;
using API.STEF.Domain.Shared.Interfaces;
using System.Linq.Expressions;

namespace API.STEF.Application.Orders.Specifications
{
    public class OrderByIdSpecification : ISpecification<Order>
    {
        public OrderByIdSpecification(int id)
        {
            Predicate = (x) => x.Id == id;
        }

        public Expression<Func<Order, bool>> Predicate { get; set; }
    }
}
