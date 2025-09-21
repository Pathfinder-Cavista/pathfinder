using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface IJobRepository
    {
        Task AddAsync(Job job, bool save = true);
        Task EditAsync(Job job, bool save = true);
        Task<Job?> GetAsync(Expression<Func<Job, bool>> expression, bool track = false);
        IQueryable<Job> GetQueryable(Expression<Func<Job, bool>> expression);
    }
}
