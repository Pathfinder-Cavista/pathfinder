using PathFinder.Application.Helpers;
using PathFinder.Domain.Entities;

namespace PathFinder.Application.DTOs
{
    public class JobDetailDto
    {
        public Guid Id { get; set; }
        public Guid RecruiterId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public DateTime? DeadLine { get; set; }
        public List<string> Requirements { get; set; } = [];
        public List<string> RequiredSkills { get; set; } = [];
        public OrganizationDto? AboutCompany { get; set; }

        public static JobDetailDto FromEntity(Job job, List<string> requirements, List<string> skills, Organization? org = null)
        {
            var data = new JobDetailDto
            {
                Id = job.Id,
                RecruiterId = job.RecruiterId,
                Title = job.Title,
                Status = job.Status.GetDescription(),
                Type = job.EmploymentType.GetDescription(),
                Level = job.Level.GetDescription(),
                Location = job.Location,
                Description = job.Description,
                DeadLine = job.ClosingDate.HasValue ? job.ClosingDate.Value.Date : null,
                Requirements = requirements,
                RequiredSkills = skills
            };

            if(org != null)
            {
                data.AboutCompany = new OrganizationDto(org.Name, org.About);
            }

            return data;
        }
    }
}
