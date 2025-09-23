using PathFinder.Domain.Enums;

namespace PathFinder.Application.Commands.Jobs
{
    public class ApplicationStatusCommand
    {
        public Guid ApplicationId { get; set; }
        public JobApplicationStatus NewStatus { get; set; }
    }
}