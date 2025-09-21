using PathFinder.Domain.Entities;

namespace PathFinder.Application.DTOs
{
    public class LeanJobDto
    {
        public Guid Id { get; set; }
        public Guid RecruiterId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime? DeadLine { get; set; }
        public DateTime DateCreated { get; set; }
        public List<string> RequiredSkills { get; set; } = [];
    }
}
