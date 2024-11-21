using API.STEF.Domain.Shared.Entities;
using System.Linq.Expressions;

namespace API.STEF.Domain.Shared.Interfaces
{
    public interface ISpecification<TEntity> where TEntity : Entity
    {
        Expression<Func<TEntity, bool>> Predicate { get; }  
    }
}
