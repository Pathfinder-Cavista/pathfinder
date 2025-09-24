using System.ComponentModel.DataAnnotations;

namespace PathFinder.Domain.Entities
{
    public class Holiday : BaseEntity
    {
        [Required]
        public DateTime Date { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Country { get; set; }
        public bool IsRecurring { get; set; }
    }
}
