using PathFinder.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PathFinder.Domain.Entities
{
    public class Job : BaseEntity
    {
        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public string? Summary { get; set; } = string.Empty;
        public JobStatus Status { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public JobLevel Level { get; set; }
        public string? Location { get; set; }

        // Nav. properties
        public string PostedByUserId { get; set; } = string.Empty;
        public RecruiterProfile? PostedBy { get; set; }
        public ICollection<JobApplication> Applications { get; set; } = [];
    }
}
