using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface IRecruiterProfileRepository
    {
        Task AddAsync(RecruiterProfile profile, bool saveNow = true);
        Task EditAsync(RecruiterProfile profile, bool saveNow = true);
        Task<RecruiterProfile?> GetAsync(Expression<Func<RecruiterProfile, bool>> expression, bool track = true, bool includeOrg = false);
    }
}
