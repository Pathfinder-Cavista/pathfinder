using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public class TalentSkillRepository : Repository<TalentSkill>, ITalentSkillRepository
    {
        public TalentSkillRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task AddRangeAsync(List<TalentSkill> skills, bool saveNow)
        {
            await InsertRangeAsync(skills, saveNow);
        }

        public async Task RemoveManyAsync(List<TalentSkill> skills, bool saveNow)
        {
            await DeleteRangeAsync(skills, saveNow);
        }

        public async Task<List<TalentSkill>> GetAsync(Expression<Func<TalentSkill, bool>> expression)
        {
            var skills =  await FindAsync(expression);
            return skills;
        }
    }
}
