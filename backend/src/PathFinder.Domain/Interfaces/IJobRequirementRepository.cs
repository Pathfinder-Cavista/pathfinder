using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface IJobRequirementRepository
    {
        Task AddRangeAsync(List<JobRequirement> requirements, bool save = true);
        Task DeleteManyAsync(List<JobRequirement> requirements, bool save = true);
        Task<List<JobRequirement>> GetAsync(Expression<Func<JobRequirement, bool>> predicate);
    }
}