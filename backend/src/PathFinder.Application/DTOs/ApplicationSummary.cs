namespace PathFinder.Application.DTOs
{
    public record ApplicationSummary
    {
        public long Applicants { get; set; }
        public long Eligible { get; set; }
        public long Interviewed { get; set; }
        public long Hired { get; set; }
    }
}