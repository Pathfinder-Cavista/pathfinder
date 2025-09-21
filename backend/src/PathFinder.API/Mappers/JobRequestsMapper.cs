using PathFinder.API.Requests.Jobs;
using PathFinder.Application.Commands.Jobs;

namespace PathFinder.API.Mappers
{
    public class JobRequestsMapper
    {
        public static PostJobCommand ToPostJobCommand(PostJobRequest request)
        {
            return new PostJobCommand
            {
                JobTitle = request.Title,
                Description = request.Description,
                DeadLine = request.DeadLine,
                Location = request.Location,
                PostNow = request.PublishNow,
                Skills = request.Skills,
                Requirements = request.Requirements,
                Level = request.Level,
                EmploymentType = request.EmploymentType
            };
        }

        public static PatchJobCommand ToPatchJobCommand(Guid id, PatchJobRequest request)
        {
            return new PatchJobCommand
            {
                Id = id,
                JobTitle = request.Title,
                Description = request.Description,
                DeadLine = request.DeadLine,
                Location = request.Location,
                PostNow = request.PublishNow,
                Skills = request.Skills,
                Requirements = request.Requirements,
                Level = request.Level,
                EmploymentType = request.EmploymentType
            };
        }

        public static JobQuery MapJobQuery(JobQueryRequest request)
        {
            return new JobQuery
            {
                Size = request.Size,
                Page = request.Page,
                Order = request.SortOrder,
                Level = request.Level,
                Status = request.Status,
                Type = request.Type,
                Search = request.Search
            };
        }
    }
}
