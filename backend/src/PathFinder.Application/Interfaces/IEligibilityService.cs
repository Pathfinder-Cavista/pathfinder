
using PathFinder.Domain.Entities;

namespace PathFinder.Application.Interfaces
{
    public interface IEligibilityService
    {
        Task EvaluateEligibility(Guid jobId, Guid talentId, double threshold = 0.7);
        Task<List<Skill>> HandleSkillsUpdate(List<string> talentSkills);
    }
}
