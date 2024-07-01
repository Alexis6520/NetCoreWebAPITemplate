using Microsoft.EntityFrameworkCore;
using Services.Repositories;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementación de repositorio genérico
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity>(DbSet<TEntity> dbSet) : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet = dbSet;

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task<TEntity> FindAsync(object[] keys, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(keys, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "", CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public virtual void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
