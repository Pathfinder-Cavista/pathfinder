using PathFinder.Domain.Enums;

namespace PathFinder.Application.DTOs
{
    public class DataloadJobApplication
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long JobId { get; set; }
        public JobApplicationStatus Status { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
