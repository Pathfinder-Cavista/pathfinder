using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface IJobSkillRepository
    {
        Task AddRangeAsync(List<JobSkill> skills, bool saveNow);
        IQueryable<JobSkill> AsQueryable(Expression<Func<JobSkill, bool>> expression);
        Task<List<JobSkill>> GetAsync(Expression<Func<JobSkill, bool>> expression);
        Task RemoveManyAsync(List<JobSkill> skills, bool saveNow);
    }
}
