using API.STEF.Domain.Shared.Entities;
using API.STEF.Domain.Shared.Interfaces;

namespace API.STEF.Domain.Contracts.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : Entity
    {
        Task DeleteAsync(ISpecification<TEntity> spec);
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity?> GetAsync(ISpecification<TEntity> spec);
        Task<IEnumerable<TEntity>> GetAsync(int pageSize = -1, int pageNumber = -1, ISpecification<TEntity> spec = null);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
