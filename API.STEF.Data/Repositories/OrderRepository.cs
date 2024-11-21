using API.STEF.Data.Context;
using API.STEF.Domain.Contracts.Repositories;
using API.STEF.Domain.OrderAggregator;
using Microsoft.EntityFrameworkCore;

namespace API.STEF.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(StefaniniContext context) : base(context)
        {
        }

        protected override IQueryable<Order> GetWithIncludes()
        {
            return _dbSet.Include(x => x.OrderItems).ThenInclude(x => x.Product);
        }
    }
}
