using System.ComponentModel.DataAnnotations;

namespace PathFinder.Domain.Entities
{
    public class Sample : BaseEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime Date { get; set; }
    }
}