using PathFinder.Domain.Enums;

namespace PathFinder.Domain.Entities
{
    public class Report : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? AssetUrl { get; set; }
        public bool IsComplete { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Running;
        public DateTime? CompletionTime { get; set; }
    }
}