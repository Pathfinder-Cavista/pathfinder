using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task AddAsync(Job job, bool save = true)
        {
            await InsertAsync(job, save);
        }

        public async Task<Job?> GetAsync(Expression<Func<Job, bool>> expression,
                                         bool track = false)
        {
            return await FindOneAsync(expression, track);
        }

        public IQueryable<Job> GetQueryable(Expression<Func<Job, bool>> expression)
        {
            return GetAsQueryable(expression);
        }

        public async Task EditAsync(Job job, bool save = true)
        {
            await UpdateAsync(job, save);
        }
    }
}
