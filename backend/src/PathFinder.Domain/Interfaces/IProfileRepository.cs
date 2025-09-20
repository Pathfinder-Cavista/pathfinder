using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface IProfileRepository
    {
        Task<RecruiterProfile?> GetRecruiterProfileAsync(Expression<Func<RecruiterProfile, bool>> expression);
        Task<TalentProfile?> GetTalentProfileAsync(Expression<Func<TalentProfile, bool>> expression);
    }
}
