using PathFinder.Domain.Enums;

namespace PathFinder.Application.DTOs
{
    public class OpenRoleDurationDto
    {
        public Guid RoleId { get; set; }
        public string RoleTitle { get; set; } = string.Empty;
        public JobStatus Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public int OpenDays { get; set; }
    }
}