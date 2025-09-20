using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    internal class JobSkillRepository : Repository<JobSkill>, IJobSkillRepository
    {
        public JobSkillRepository(AppDbContext context) : base(context)
        {
            
        }
        public async Task AddRangeAsync(List<JobSkill> skills, bool saveNow)
        {
            await InsertRangeAsync(skills, saveNow);
        }

        public async Task RemoveManyAsync(List<JobSkill> skills, bool saveNow)
        {
            await DeleteRangeAsync(skills, saveNow);
        }

        public async Task<List<JobSkill>> GetAsync(Expression<Func<JobSkill, bool>> expression)
        {
            var skills = await FindAsync(expression);
            return skills;
        }
    }
}
