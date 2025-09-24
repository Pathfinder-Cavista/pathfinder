namespace PathFinder.Application.DTOs
{
    public class YearlyApplicationTrendsDto
    {
        public int MonthKey { get; set; }
        public string Month { get; set; } = string.Empty;
        public long Applications { get; set; }
    }
}