using PathFinder.Application.Commands.Jobs;
using PathFinder.Application.Responses;

namespace PathFinder.Application.Interfaces
{
    public interface IJobService
    {
        Task<ApiBaseResponse> DeprecateJobAsync(Guid id);
        Task<ApiBaseResponse> GetByIdAsync(Guid id);
        ApiBaseResponse GetPaginatedJobs(JobQuery query);
        Task<ApiBaseResponse> PatchJobAsync(PatchJobCommand command);
        Task<ApiBaseResponse> PostJobAsync(PostJobCommand command);
    }
}