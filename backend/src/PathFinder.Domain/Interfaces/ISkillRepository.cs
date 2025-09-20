using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface ISkillRepository
    {
        Task AddRangeAsync(List<Skill> skills, bool save = true);
        Task AddRangeAsync(List<TalentSkill> skills, bool save = true);
        Task AddRangeAsync(List<JobSkill> skills, bool save = true);
        IQueryable<Skill> AsQueryable(Expression<Func<Skill, bool>> expression);
        Task<List<JobSkill>> GetJobSkillsAsync(Expression<Func<JobSkill, bool>> expression);
        Task<List<TalentSkill>> GetTalentSkillsAsync(Expression<Func<TalentSkill, bool>> expression);
    }
}
