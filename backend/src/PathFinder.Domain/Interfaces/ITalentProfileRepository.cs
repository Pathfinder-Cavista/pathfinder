using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface ITalentProfileRepository
    {
        Task AddAsync(TalentProfile profile, bool saveNow = true);
        IQueryable<TalentProfile> AsQueryable(Expression<Func<TalentProfile, bool>> expression);
        Task EditAsync(TalentProfile profile, bool saveNow = true);
        Task<TalentProfile?> GetAsync(Expression<Func<TalentProfile, bool>> expression, bool track = true);
    }
}
