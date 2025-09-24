using PathFinder.Domain.Enums;

namespace PathFinder.Application.DTOs
{
    public class ApplicationStatusDistributionDto
    {
        public JobApplicationStatus Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public long Count { get; set; }
    }
}