using PathFinder.Domain.Enums;

namespace PathFinder.Application.DTOs
{
    public class HireRateByJobTypeDto
    {
        public EmploymentType JobType { get; set; }
        public string JobTypeText { get; set; } = string.Empty;
        public long Hires { get; set; }
    }
}
