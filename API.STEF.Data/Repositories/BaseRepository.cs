using API.STEF.Data.Context;
using API.STEF.Domain.Contracts.Repositories;
using API.STEF.Domain.Shared.Entities;
using API.STEF.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.STEF.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly StefaniniContext _context;

        public BaseRepository(StefaniniContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        protected abstract IQueryable<TEntity> GetWithIncludes();

        public virtual async Task<IEnumerable<TEntity>> GetAsync(int pageSize = -1, int pageNumber = -1, ISpecification<TEntity> spec = null)
        {
            var query = GetWithIncludes();
            
            if(spec != null)
            {
                query = query.Where(spec.Predicate);
            }

            if (pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize);
            }

            if (pageSize > 0)
            {
                query = query.Take(pageSize);
            }


            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await GetWithIncludes().ToListAsync();
        }

        public virtual async Task<TEntity?> GetAsync(ISpecification<TEntity> spec)
        {
            var entity = await GetWithIncludes().FirstOrDefaultAsync(spec.Predicate);
            return entity;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(ISpecification<TEntity> spec)
        {
            var entity = await GetAsync(spec);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
