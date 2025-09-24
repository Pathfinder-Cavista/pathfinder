namespace PathFinder.API.Requests
{
    public class HolidayWriteRequest
    {
        public DateTime Date { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Country { get; set; }
        public bool IsRecurring { get; set; }
    }
}
