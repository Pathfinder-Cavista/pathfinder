using PathFinder.Application.Commands.Jobs;
using PathFinder.Application.Helpers;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;

namespace PathFinder.Application.Mappers
{
    public class JobCommandMapper
    {
        public static Job ToJobModel(PostJobCommand command,
                                     string userId,
                                     bool postNow = true)
        {
            return new Job
            {
                Title = command.JobTitle,
                Description = command.Description,
                Level = command.Level,
                EmploymentType = command.EmploymentType,
                Location = command.Location,
                ClosingDate = command.DeadLine.HasValue ? command.DeadLine.Value.ToDayEnd() : null,
                PostedByUserId = userId,
                Status = postNow ? JobStatus.Published : JobStatus.Draft
            };
        }
    }
}
