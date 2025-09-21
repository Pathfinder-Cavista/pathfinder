using PathFinder.Application.Commands.Jobs;
using PathFinder.Application.DTOs;
using PathFinder.Domain.Entities;

namespace PathFinder.Application.Helpers
{
    public static class RecordFilterExtension
    {
        public static IQueryable<Job> Filter(this IQueryable<Job> jobs, JobQuery jobQuery)
        {
            if (!string.IsNullOrWhiteSpace(jobQuery.Search))
            {
                jobs = jobs.Where(j => 
                    j.Title.ToLower().Contains(jobQuery.Search.ToLower()) || 
                    j.Description.ToLower().Contains(jobQuery.Search.ToLower()));
            }
            if (jobQuery.Level.HasValue)
            {
                jobs = jobs.Where(j => j.Level == jobQuery.Level.Value);
            }
            if (jobQuery.Type.HasValue)
            {
                jobs = jobs.Where(j => j.EmploymentType == jobQuery.Type.Value);
            }
            if (jobQuery.Status.HasValue)
            {
                jobs = jobs.Where(j => j.Status == jobQuery.Status.Value);
            }

            return jobs;
        }

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