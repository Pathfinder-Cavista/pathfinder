using Microsoft.EntityFrameworkCore;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    internal class JobSkillRepository : IJobSkillRepository
    {
        private readonly AppDbContext _context;

        public JobSkillRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(List<JobSkill> skills, bool saveNow)
        {
            await _context.AddRangeAsync(skills);
            if (saveNow)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveManyAsync(List<JobSkill> skills, bool saveNow)
        {
            _context.RemoveRange(skills);
            if (saveNow)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<JobSkill>> GetAsync(Expression<Func<JobSkill, bool>> expression)
        {
            return await _context.Set<JobSkill>()
                 .Where(expression)
                 .ToListAsync();
        }

        public IQueryable<JobSkill> AsQueryable(Expression<Func<JobSkill, bool>> expression)
        {
            return _context.Set<JobSkill>()
                .AsQueryable()
                .Where(expression);
        }
    }
}
