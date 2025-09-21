using PathFinder.Application.Commands.Jobs;
using PathFinder.Application.Helpers;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;

namespace PathFinder.Application.Mappers
{
    public class JobCommandMapper
    {
        public static Job ToJobModel(PostJobCommand command,
                                     Guid recruiterId)
        {
            return new Job
            {
                Title = command.JobTitle,
                Description = command.Description,
                Level = command.Level,
                EmploymentType = command.EmploymentType,
                Location = command.Location,
                ClosingDate = command.DeadLine.HasValue ? command.DeadLine.Value.ToDayEnd() : null,
                RecruiterId = recruiterId,
                Status = command.PostNow ? JobStatus.Published : JobStatus.Draft
            };
        }

        public static void PatchModel(Job existingJob, PatchJobCommand command)
        {
            existingJob.Title = command.JobTitle;
            existingJob.Description = command.Description;
            existingJob.Level = command.Level;
            existingJob.EmploymentType = command.EmploymentType;
            existingJob.Location = command.Location;
            existingJob.ClosingDate = command.DeadLine;
            existingJob.ModifiedAt = DateTime.UtcNow;
        }
    }
}
