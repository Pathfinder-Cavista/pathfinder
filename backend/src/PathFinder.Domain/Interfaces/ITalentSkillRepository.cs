using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface ITalentSkillRepository
    {
        Task AddRangeAsync(List<TalentSkill> skills, bool saveNow);
        Task<List<TalentSkill>> GetAsync(Expression<Func<TalentSkill, bool>> expression);
        Task RemoveManyAsync(List<TalentSkill> skills, bool saveNow);
    }
}
