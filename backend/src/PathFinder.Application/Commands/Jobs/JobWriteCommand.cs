using PathFinder.Domain.Enums;

namespace PathFinder.Application.Commands.Jobs
{
    public abstract class JobWriteCommand
    {
        public string JobTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DeadLine { get; set; }
        public ICollection<string> Requirements { get; set; } = [];
        public bool PostNow { get; set; } = true;
        public EmploymentType EmploymentType { get; set; }
        public JobLevel Level { get; set; }
        public string? Location { get; set; }
        public List<string> Skills { get; set; } = [];
    }
}
