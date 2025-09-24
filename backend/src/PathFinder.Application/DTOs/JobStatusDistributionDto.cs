using PathFinder.Domain.Enums;

namespace PathFinder.Application.DTOs
{
    public class JobStatusDistributionDto
    {
        public JobStatus Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public long Count { get; set; }
    }
}
