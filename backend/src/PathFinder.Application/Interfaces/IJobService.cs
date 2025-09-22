using Microsoft.AspNetCore.Http;
using PathFinder.Application.Commands.Jobs;
using PathFinder.Application.Queries;
using PathFinder.Application.Queries.Jobs;
using PathFinder.Application.Responses;

namespace PathFinder.Application.Interfaces
{
    public interface IJobService
    {
        Task<ApiBaseResponse> ApplyAsync(Guid id);
        Task<ApiBaseResponse> DeprecateJobAsync(Guid id);
        Task<ApiBaseResponse> GetApplicationAsync(Guid applicationId, Guid jobId);
        Task<ApiBaseResponse> GetByIdAsync(Guid id);
        Task<ApiBaseResponse> GetJobApplicationsAsync(ApplicationQueries queries);
        ApiBaseResponse GetPaginatedJobs(JobQuery query);
        Task<ApiBaseResponse> GetTalentJobApplicationsAsync(PageQuery queries);
        Task<ApiBaseResponse> PatchJobAsync(PatchJobCommand command);
        Task<ApiBaseResponse> PostJobAsync(PostJobCommand command);
    }
}