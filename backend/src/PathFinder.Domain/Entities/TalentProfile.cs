namespace PathFinder.Domain.Entities
{
    public class TalentProfile : BaseEntity
    {
        public string? Location { get; set; } = string.Empty;
        public string? ResumeUrl { get; set; } = string.Empty;
        public string? ResumePublicId { get; set; }
        public string? Summary { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }

        // Nav. properties
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }
        public ICollection<JobApplication> Applications { get; set; } = [];
        public ICollection<TalentSkill> Skills { get; set; } = [];
    }
}
