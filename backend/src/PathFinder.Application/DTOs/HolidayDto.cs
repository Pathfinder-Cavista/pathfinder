namespace PathFinder.Application.DTOs
{
    public class HolidayDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsRecurring { get; set; }
        public string? Country { get; set; }
    }
}
