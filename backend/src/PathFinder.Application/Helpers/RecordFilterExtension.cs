using PathFinder.Application.DTOs;
using PathFinder.Domain.Entities;

namespace PathFinder.Application.Helpers
{
    public static class RecordFilterExtension
    {
        public static IQueryable<LeanJobDto> AsLeanJobDto(this IQueryable<Job> jobs)
        {
            var query = from job in jobs
                        select new LeanJobDto
                        {
                            Id = job.Id,
                            Title = job.Title,
                            Description = job.Description,
                            DeadLine = job.ClosingDate,
                            Level = job.Level.GetDescription(),
                            Type = job.EmploymentType.GetDescription(),
                            Status = job.Status.GetDescription(),
                            Location = job.Location,
                            RecruiterId = job.RecruiterId
                        };

            return query;
        }
    }
}