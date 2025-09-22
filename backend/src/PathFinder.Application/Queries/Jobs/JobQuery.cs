using PathFinder.Domain.Enums;

namespace PathFinder.Application.Queries.Jobs
{
    public class JobQuery : PageQuery
    {
        public string? Search { get; set; }
        public string Order { get; set; } = "DESC";
        public JobStatus? Status { get; set; }
        public EmploymentType? Type { get; set; }
        public JobLevel? Level { get; set; }
    }
}
