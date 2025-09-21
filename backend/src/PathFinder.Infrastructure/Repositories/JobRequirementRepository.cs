using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public class JobRequirementRepository : Repository<JobRequirement>, IJobRequirementRepository
    {
        public JobRequirementRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task AddRangeAsync(List<JobRequirement> requirements, bool save = true)
        {
            await InsertRangeAsync(requirements, save);
        }

        public async Task DeleteManyAsync(List<JobRequirement> requirements, bool save = true)
        {
            await DeleteRangeAsync(requirements, save);
        }

        public async Task<List<JobRequirement>> GetAsync(Expression<Func<JobRequirement, bool>> predicate)
        {
            return await FindAsync(predicate);
        }
    }
}
