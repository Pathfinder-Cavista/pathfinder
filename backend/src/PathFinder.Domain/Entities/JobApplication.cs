using PathFinder.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PathFinder.Domain.Entities
{
    public class JobApplication : BaseEntity
    {
        public Guid JobId { get; set; }
        public Job? Job { get; set; }

        public Guid TalentId { get; set; }
        public TalentProfile? Talent { get; set; }

        [Required]
        public string? ResumeUrl { get; set; }
        public bool IsEligible { get; set; }
        public double AttainedThreshold { get; set; }
        public JobApplicationStatus Status { get; set; } = JobApplicationStatus.Applied;
    }
}
