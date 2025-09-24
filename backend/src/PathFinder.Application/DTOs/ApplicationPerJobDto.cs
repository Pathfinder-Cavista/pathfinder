namespace PathFinder.Application.DTOs
{
    public class ApplicationPerJobDto
    {
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public long Count { get; set; }
    }
}
