namespace PathFinder.Application.DTOs
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? AssetUrl { get; set; }
        public string UserName { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? CompleteTime { get; set; }
    }
}
