using System.ComponentModel.DataAnnotations;

namespace PathFinder.Domain.Entities
{
    public class RecruiterProfile : BaseEntity
    {
        [Required]
        public string? Title { get; set; } = string.Empty;

        // Nav properties
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }

        public ICollection<Job> PostedJobs { get; set; } = [];
    }
}
