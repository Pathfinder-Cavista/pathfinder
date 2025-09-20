using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface ISkillRepository
    {
        Task AddRangeAsync(List<Skill> skills, bool save = true);
        IQueryable<Skill> AsQueryable(Expression<Func<Skill, bool>> expression);
    }
}
