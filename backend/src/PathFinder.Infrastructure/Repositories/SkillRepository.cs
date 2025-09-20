using Microsoft.EntityFrameworkCore;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        public SkillRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Skill> AsQueryable(Expression<Func<Skill, bool>> expression)
            => GetAsQueryable(expression);

        public async Task AddRangeAsync(List<Skill> skills, bool save = true)
        {
            await InsertRangeAsync(skills, save);
        }
    }
}
