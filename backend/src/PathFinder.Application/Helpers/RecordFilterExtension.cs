using PathFinder.Application.Commands.Jobs;
using PathFinder.Application.DTOs;
using PathFinder.Domain.Entities;

namespace PathFinder.Application.Helpers
{
    public static class RecordFilterExtension
    {
        private const string DefaultOrder = "DESC";

        public static IQueryable<LeanJobDto> Filter(this IQueryable<LeanJobDto> jobs, JobQuery jobQuery)
        {
            if (!string.IsNullOrWhiteSpace(jobQuery.Search))
            {
                jobs = jobs.Where(j => 
                    j.Title.ToLower().Contains(jobQuery.Search.ToLower()) || 
                    j.Description.ToLower().Contains(jobQuery.Search.ToLower()));
            }
            if (jobQuery.Level.HasValue)
            {
                jobs = jobs.Where(j => j.Level == jobQuery.Level.Value.GetDescription());
            }
            if (jobQuery.Type.HasValue)
            {
                jobs = jobs.Where(j => j.Type == jobQuery.Type.Value.GetDescription());
            }
            if (jobQuery.Status.HasValue)
            {
                jobs = jobs.Where(j => j.Status == jobQuery.Status.Value.GetDescription());
            }

            return jobs;
        }

        public static IQueryable<LeanJobDto> AsLeanJobDto(this IQueryable<Job> jobs, 
                                                            IQueryable<JobSkill> jobSkillQuery,
                                                            IQueryable<Skill> skillQuery,
                                                            JobQuery filter)
        {
            var query = from job in jobs
                        join js in jobSkillQuery on job.Id equals js.JobId
                        join s in skillQuery on js.SkillId equals s.Id
                        group s by new { job.Id, job.Title, job.Description, job.EmploymentType, 
                            job.Level, job.Status, job.RecruiterId, job.Location, job.ClosingDate, 
                            job.CreatedAt } into grp
                        select new LeanJobDto
                        {
                            Id = grp.Key.Id,
                            Title = grp.Key.Title,
                            Description = grp.Key.Description,
                            DeadLine = grp.Key.ClosingDate,
                            Level = grp.Key.Level.GetDescription(),
                            Type = grp.Key.EmploymentType.GetDescription(),
                            Status = grp.Key.Status.GetDescription(),
                            Location = grp.Key.Location,
                            RecruiterId = grp.Key.RecruiterId,
                            DateCreated = grp.Key.CreatedAt,
                            RequiredSkills = grp.Select(sk => sk.Name).ToList(),
                        };

            query = filter.Order.Equals(DefaultOrder, StringComparison.OrdinalIgnoreCase) ?
                query.OrderByDescending(j => j.DateCreated) : query.OrderBy(j => j.DateCreated);

            return query.Filter(filter);
        }
    }
}