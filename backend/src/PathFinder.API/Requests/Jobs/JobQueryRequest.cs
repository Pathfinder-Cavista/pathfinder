using PathFinder.Domain.Enums;

namespace PathFinder.API.Requests.Jobs
{
    public class JobQueryRequest : PageQueryRequest
    {
        public string SortOrder { get; set; } = "DESC";
        public string? Search { get; set; }
        public JobStatus? Status { get; set; }
        public EmploymentType? Type { get; set; }
        public JobLevel? Level { get; set; }
    }
}
