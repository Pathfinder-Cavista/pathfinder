using Microsoft.EntityFrameworkCore;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        private readonly AppDbContext _dbContext;

        public SkillRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Skill> AsQueryable(Expression<Func<Skill, bool>> expression)
            => GetAsQueryable(expression);

        public async Task AddRangeAsync(List<Skill> skills, bool save = true)
        {
            await InsertRangeAsync(skills, save);
        }

        public async Task AddRangeAsync(List<TalentSkill> skills, bool save = true)
        {
            await _dbContext.AddRangeAsync(skills);
            if (save)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task AddRangeAsync(List<JobSkill> skills, bool save = true)
        {
            await _dbContext.AddRangeAsync(skills);
            if (save)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<TalentSkill>> GetTalentSkillsAsync(Expression<Func<TalentSkill, bool>> expression)
        {
            var skills = await _dbContext.TalentSkills
                .Include(ts => ts.Skill)
                .Where(expression)
                .ToListAsync();

            return skills;
        }

        public async Task<List<JobSkill>> GetJobSkillsAsync(Expression<Func<JobSkill, bool>> expression)
        {
            var skills = await _dbContext.JobSkills
                .Include(ts => ts.Skill)
                .Where(expression)
                .ToListAsync();

            return skills;
        }
    }
}
