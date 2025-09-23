using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public class JobApplicationRepository : Repository<JobApplication>, IJobApplicationRepository
    {
        public JobApplicationRepository(AppDbContext dbContext) : base(dbContext) { }
        
        public async Task ApplyAsync(JobApplication jobApplication, bool save = true)
        {
            await InsertAsync(jobApplication, save);
        }

        public IQueryable<JobApplication> AsQueryable(Expression<Func<JobApplication, bool>> expression)
        {
            return GetAsQueryable(expression);
        }

        public async Task<JobApplication?> GetAsync(Expression<Func<JobApplication, bool>> expression,
                                                    bool track = false)
        {
            return await FindOneAsync(expression, track);
        }

        public async Task EditAsync(JobApplication application,
                                                    bool save = true)
        {
            await UpdateAsync(application, save);
        }
    }
}
