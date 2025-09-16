using Microsoft.EntityFrameworkCore;
using PathFinder.Domain.Entities;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public abstract class Repository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertAsync(T entity,
                                      bool saveNow = true,
                                      CancellationToken cancellation = default)
        {
            await _dbContext.AddAsync(entity, cancellation);
            if (saveNow)
            {
                await _dbContext.SaveChangesAsync(cancellation);
            }
        }

        public async Task UpdateAsync(T entity,
                                      bool saveNow = true,
                                      CancellationToken cancellation = default)
        {
            _dbContext.Update(entity);
            if (saveNow)
            {
                await _dbContext.SaveChangesAsync(cancellation);
            }
        }

        public async Task DeleteAsync(T entity,
                                      bool saveNow = true,
                                      CancellationToken cancellation = default)
        {
            _dbContext.Remove(entity);
            if (saveNow)
            {
                await _dbContext.SaveChangesAsync(cancellation);
            }
        }

        public IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>()
                .AsQueryable()
                .Where(expression);
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> expression,
                                        bool trackChanges = false,
                                        CancellationToken cancellation = default)
        {
            return trackChanges ? 
                await _dbContext.Set<T>().FirstOrDefaultAsync(expression, cancellation) :
                await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression, cancellation);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression,
                                             CancellationToken cancellation = default)
        {
            return await _dbContext.Set<T>()
                .Where(expression)
                .ToListAsync(cancellation);
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> expression,
                                           CancellationToken cancellation = default)
        {
            return await _dbContext.Set<T>()
                .LongCountAsync(expression, cancellation);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression,
                                            CancellationToken cancellation = default)
        {
            return await _dbContext.Set<T>()
                .AnyAsync(expression, cancellation);
        }
    }
}