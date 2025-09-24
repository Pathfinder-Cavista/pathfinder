using PathFinder.Domain.Enums;

namespace PathFinder.Application.DTOs
{
    public class DataloadJob
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public EmploymentType Type { get; set; }
        public JobLevel Level { get; set; }
        public string Location { get; set; } = string.Empty;
        public List<string> Skills { get; set; } = [];
        public List<string> Requirements { get; set; } = [];
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public JobStatus Status { get; set; }
    }
}
