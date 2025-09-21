using System.ComponentModel.DataAnnotations;

namespace PathFinder.Domain.Entities
{
    public class JobRequirement : BaseEntity
    {
        [Required]
        public string Requirement { get; set; } = string.Empty;

        public Guid JobId { get; set; }
        public Job? Job { get; set; }
    }
}
