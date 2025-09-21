using PathFinder.Domain.Enums;

namespace PathFinder.Application.Commands.Jobs
{
    public class ChangeJobStatusCommand
    {
        public JobStatus Status { get; set; }
    }
}