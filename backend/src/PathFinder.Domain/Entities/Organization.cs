using System.ComponentModel.DataAnnotations;

namespace PathFinder.Domain.Entities
{
    public class Organization : BaseEntity
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        public string Vision { get; set; } = string.Empty;
        [StringLength(500)]
        public string Mission { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;

        public ICollection<RecruiterProfile> Members { get; set; } = [];
    }
}
