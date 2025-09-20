using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PathFinder.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string? ProfilePhoto { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreratedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public TalentProfile? Talent { get; set; }
        public RecruiterProfile? Recruiter { get; set; }
    }
}
