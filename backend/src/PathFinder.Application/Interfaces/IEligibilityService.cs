
namespace PathFinder.Application.Interfaces
{
    public interface IEligibilityService
    {
        Task EvaluateEligibility(Guid jobId, Guid talentId, double threshold = 0.7);
    }
}
