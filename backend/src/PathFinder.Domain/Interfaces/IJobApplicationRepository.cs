using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface IJobApplicationRepository
    {
        Task ApplyAsync(JobApplication jobApplication, bool save = true);
        IQueryable<JobApplication> AsQueryable(Expression<Func<JobApplication, bool>> expression);
        Task EditAsync(JobApplication application, bool save = true);
        Task<JobApplication?> GetAsync(Expression<Func<JobApplication, bool>> expression, bool track = false);
    }
}
