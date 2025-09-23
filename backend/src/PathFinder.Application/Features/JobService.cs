using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathFinder.Application.Commands.Jobs;
using PathFinder.Application.DTOs;
using PathFinder.Application.Helpers;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Mappers;
using PathFinder.Application.Queries;
using PathFinder.Application.Queries.Jobs;
using PathFinder.Application.Responses;
using PathFinder.Application.Validations.Jobs;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;
using PathFinder.Domain.Interfaces;
using System.Buffers.Text;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PathFinder.Application.Features
{
    public class JobService : IJobService
    {
        private readonly IRepositoryManager _repository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEligibilityService _eligibility;

        public JobService(IRepositoryManager repository,
                          IHttpContextAccessor contextAccessor,
                          UserManager<AppUser> userManager,
                          IEligibilityService eligibility)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _eligibility = eligibility;
        }

        public async Task<ApiBaseResponse> PostJobAsync(PostJobCommand command)
        {
            var validator = new JobCommandsValidator().Validate(command);
            if (!validator.IsValid)
            {
                return new BadRequestResponse(validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid inputs");
            }

            var loggedInUserId = AccountHelpers
                .GetLoggedInUserId(_contextAccessor.HttpContext.User);
            if (string.IsNullOrWhiteSpace(loggedInUserId))
            {
                return new ForbiddenResponse("You're not authorized to perform this operation.");
            }

            var recruiter = await _repository.RecruiterProfile
                .GetAsync(r => r.UserId == loggedInUserId, false, true);
            if(recruiter == null)
            {
                return new NotFoundResponse("No recruiter profile found for the logged in user");
            }

            var jobToAdd = JobCommandMapper.ToJobModel(command, recruiter.Id);
            await _repository.Job.AddAsync(jobToAdd);

            var requirements = GetRequirements(jobToAdd.Id, command.Requirements.ToList());
            await _repository.JobRequirement
                .AddRangeAsync(requirements);

            var requiredSkills = await PopulateJobSkills(jobToAdd.Id, command.Skills.ToList(), true);
            await _repository.JobSkill
                .AddRangeAsync(requiredSkills, true);

            return new OkResponse<EntityIdDto>(new EntityIdDto(jobToAdd.Id), string.Format("Job successfully {0}.", command.PostNow ? "posted" : "added to drafts"));
        }

        public async Task<ApiBaseResponse> PatchJobAsync(PatchJobCommand command)
        {
            var validator = new JobCommandsValidator().Validate(command);
            if (!validator.IsValid)
            {
                return new BadRequestResponse(validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid inputs");
            }

            var userClaim = _contextAccessor.HttpContext.User;
            var loggedInUserId = AccountHelpers.GetLoggedInUserId(userClaim);

            if (string.IsNullOrWhiteSpace(loggedInUserId))
            {
                return new ForbiddenResponse("You're not authorized to perform this operation.");
            }

            var recruiter = await _repository.RecruiterProfile
                .GetAsync(r => r.UserId == loggedInUserId, false, true);
            if (recruiter == null)
            {
                return new NotFoundResponse("No recruiter profile found for the logged in user");
            }

            var existingJob = await _repository.Job
                .GetAsync(j => j.Id == command.Id && !j.IsDeprecated);
            if (existingJob == null)
            {
                return new NotFoundResponse("Job record not found");
            }

            if(recruiter.Id != existingJob.RecruiterId && !AccountHelpers.IsInRole(userClaim, Roles.Admin.GetDescription()))
            {
                return new ForbiddenResponse("You don't have enough permissions to perform this operation");
            }

            JobCommandMapper.PatchModel(existingJob, command);
            await _repository.Job.EditAsync(existingJob);

            await UpdateJobRequirements(existingJob.Id, [.. command.Requirements]);
            var requiredSkills = await PopulateJobSkills(existingJob.Id, [.. command.Skills], false);
            await _repository.JobSkill
                .AddRangeAsync(requiredSkills, true);

            return new OkResponse<EntityIdDto>(new EntityIdDto(existingJob.Id), "Job successfully updated.");
        }

        public async Task<ApiBaseResponse> DeprecateJobAsync(Guid id)
        {
            var userClaim = _contextAccessor.HttpContext.User;
            var loggedInUserId = AccountHelpers.GetLoggedInUserId(userClaim);

            if (string.IsNullOrWhiteSpace(loggedInUserId))
            {
                return new ForbiddenResponse("You're not authorized to perform this operation.");
            }

            var recruiter = await _repository.RecruiterProfile
                .GetAsync(r => r.UserId == loggedInUserId, false, true);
            if (recruiter == null)
            {
                return new NotFoundResponse("No recruiter profile found for the loggedin user");
            }

            var existingJob = await _repository.Job
                .GetAsync(j => j.Id == id && !j.IsDeprecated);
            if (existingJob == null)
            {
                return new NotFoundResponse("Job record not found");
            }

            if (recruiter.Id != existingJob.RecruiterId && !AccountHelpers.IsInRole(userClaim, Roles.Admin.GetDescription()))
            {
                return new ForbiddenResponse("You don't have enough permissions to perform this operation");
            }

            existingJob.IsDeprecated = true;
            existingJob.ModifiedAt = DateTime.UtcNow;
            await _repository.Job.EditAsync(existingJob);

            return new OkResponse<EntityIdDto>(new EntityIdDto(existingJob.Id), "Job successfully deleted.");
        }

        public async Task<ApiBaseResponse> GetByIdAsync(Guid id)
        {
            var jobs = await _repository.Job
                .GetQueryable(_ => true).ToListAsync();

            var job = await _repository.Job
                .GetQueryable(j => j.Id ==  id && !j.IsDeprecated)
                .Include(j => j.RequiredSkills)
                .Include(j => j.Requirements)
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return new NotFoundResponse("Job record not found");
            }

            var recruiter = await _repository.RecruiterProfile
                .GetAsync(r => r.Id == job.RecruiterId, false, true);

            var requirements = job.Requirements
                .Select(r => r.Requirement)
                .ToList();

            var skillId = job.RequiredSkills
                .Select(j => j.SkillId)
                .ToList();

            var skills = await _repository.Skill
                .AsQueryable(s => skillId.Contains(s.Id))
                .Select(s => s.Name).ToListAsync();

            var data = JobDetailDto.FromEntity(job, requirements, skills, recruiter?.Organization);
            return new OkResponse<JobDetailDto>(data);
        }

        public async Task<ApiBaseResponse> GetJobForDashboard(Guid id)
        {
            var jobs = await _repository.Job
                .GetQueryable(_ => true).ToListAsync();

            var job = await _repository.Job
                .GetQueryable(j => j.Id == id && !j.IsDeprecated)
                .Include(j => j.RequiredSkills)
                .Include(j => j.Requirements)
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return new NotFoundResponse("Job record not found");
            }

            var recruiter = await _userManager.Users
                .Include(u => u.Recruiter)
                .FirstOrDefaultAsync(u => u.Recruiter != null && u.Recruiter.Id == job.RecruiterId);

            var requirements = job.Requirements
                .Select(r => r.Requirement)
                .ToList();

            var skillId = job.RequiredSkills
                .Select(j => j.SkillId)
                .ToList();

            var skills = await _repository.Skill
                .AsQueryable(s => skillId.Contains(s.Id))
                .Select(s => s.Name).ToListAsync();

            var applications = await _repository.Application
                .AsQueryable(a => a.JobId == job.Id)
                .ToListAsync();

            var interviewedCount = applications
                .LongCount(ap => ap.Status == JobApplicationStatus.Interviewed || 
                    ap.Status == JobApplicationStatus.OfferExtended || 
                    ap.Status == JobApplicationStatus.Hired);
            var eligibleCount = applications
                .LongCount(ap => ap.IsEligible);
            var hiredCount = applications
                .LongCount(ap => ap.Status == JobApplicationStatus.Hired);

            return new OkResponse<JobDetailsForDashboardDto>(
                JobDetailsForDashboardDto.FromEntity(job, requirements, skills, new ApplicationSummary
                {
                    Recruiter = $"{recruiter?.FirstName} {recruiter?.LastName}",
                    Applicants = applications.LongCount(),
                    Eligible = eligibleCount,
                    Interviewed = interviewedCount,
                    Hired = hiredCount
                }));
        }

        public ApiBaseResponse GetPaginatedJobs(JobQuery query)
        {
            var jobs = _repository.Job
                .GetQueryable(j => !j.IsDeprecated);

            var jobSkillsQuery = _repository.JobSkill.AsQueryable(_ => true);
            var skillsQuery = _repository.Skill.AsQueryable(s => !s.IsDeprecated);

            var data = jobs.AsLeanJobDto(jobSkillsQuery, skillsQuery, query)
                .Paginate(query.Page, query.Size);

            return new OkResponse<Paginator<LeanJobDto>>(data);
        }

        public async Task<ApiBaseResponse> ApplyAsync(Guid id)
        {
            var loggedInUserId = AccountHelpers.GetLoggedInUserId(_contextAccessor.HttpContext.User);
            if(string.IsNullOrWhiteSpace(loggedInUserId))
            {
                return new BadRequestResponse("Invalid user login.");
            }

            var talent = await _userManager.Users
                .Include(u => u.Talent)
                .FirstOrDefaultAsync(u => u.Id == loggedInUserId);

            if(talent == null || talent.Talent == null)
            {
                return new NotFoundResponse("User not found");
            }

            if (string.IsNullOrWhiteSpace(talent.Talent.ResumeUrl))
            {
                return new BadRequestResponse("Please upload a CV to apply for this job");
            }

            var job = await _repository.Job.GetAsync(j => j.Id == id);
            if (job == null)
            {
                return new NotFoundResponse("Job not found");
            }

            if(job.Status != JobStatus.Published)
            {
                return new ForbiddenResponse("This job is no longer open for applications");
            }

            var existingApplication = await _repository.Application
                .GetAsync(a => a.JobId == id && a.TalentId == talent.Talent.Id);
            if(existingApplication != null)
            {
                return new ForbiddenResponse("Already applied for this job");
            }

            var application = new JobApplication
            {
                JobId = job.Id,
                TalentId = talent.Talent.Id,
                ResumeUrl = talent.Talent.ResumeUrl
            };

            await _repository.Application.ApplyAsync(application);
            // TODO: Alert the user and the Recruiter
            BackgroundJob.Enqueue<IEligibilityService>(hf 
                => hf.EvaluateEligibility(job.Id, application.TalentId, 0.7));
            return new OkResponse<ApplicationDto>(new ApplicationDto(job.Id, application.Id));
        }

        public async Task<ApiBaseResponse> GetApplicationAsync(Guid applicationId, Guid jobId)
        {
            var isATalent = AccountHelpers
                .IsInRole(_contextAccessor.HttpContext.User, Roles.Talent.GetDescription());

            var talentId = Guid.Empty;
            var userId = AccountHelpers.GetLoggedInUserId(_contextAccessor.HttpContext.User);
            if (isATalent)
            {
                var talent = await _repository.TalentProfile
                    .GetAsync(p => p.UserId == userId && !string.IsNullOrEmpty(userId));
                if(talent != null)
                {
                    talentId = talent.Id;
                }
            }

            var application = await _repository.Application
                .GetAsync(a => a.Id == applicationId && a.JobId == jobId && 
                    (!isATalent || (isATalent && talentId != Guid.Empty && a.TalentId == talentId)));

            if(application == null)
            {
                return new NotFoundResponse("Job application not found");
            }

            var job = await _repository.Job
                .GetAsync(j => j.Id == application.JobId);
            if(job == null)
            {
                return new NotFoundResponse("No Job found for this application");
            }

            return new OkResponse<ApplicationDataDto>(new ApplicationDataDto
            {
                Id = application.Id,
                JobId = job.Id,
                ResumeUrl = application.ResumeUrl,
                ApplicationDate = application.CreatedAt,
                TalentId = application.TalentId,
                JobTitle = job.Title,
                JobDescription = job.Description,
                JobStatus = job.Status.GetDescription(),
                JobType = job.EmploymentType.GetDescription()
            });
        }

        public async Task<ApiBaseResponse> GetJobApplicationsAsync(ApplicationQueries queries)
        {
            var job = await _repository.Job
               .GetAsync(j => j.Id == queries.JobId);
            if (job == null)
            {
                return new NotFoundResponse("No job record found for this Id");
            }

            var users = _userManager.Users
                .Include(u => u.Talent)
                .Where(u => u.Talent != null);

            var talents = _repository.TalentProfile
                .AsQueryable(x => !x.IsDeprecated);

            var applications = _repository.Application
                .AsQueryable(a => a.JobId == queries.JobId)
                .AsApplicationsDto(job, users, talents)
                .Paginate(queries.Page, queries.Size);

            return new OkResponse<Paginator<AdminApplicationDataDto>>(applications);
        }

        public async Task<ApiBaseResponse> GetTalentJobApplicationsAsync(PageQuery queries)
        {
            var userId = AccountHelpers.GetLoggedInUserId(_contextAccessor.HttpContext.User);
            var user = await _userManager.Users
                .Include(u => u.Talent)
                .FirstOrDefaultAsync(u => u.Id == userId && !string.IsNullOrEmpty(userId));

            if(user == null || user.Talent == null)
            {
                return new NotFoundResponse("User not found");
            }

            var jobs = _repository.Job
                .GetQueryable(_ => true);

            var applications = _repository.Application
                .AsQueryable(a => a.TalentId == user.Talent.Id)
                .AsApplicationsDto(jobs, user)
                .Paginate(queries.Page, queries.Size);

            return new OkResponse<Paginator<ApplicationDataDto>>(applications);
        }

        public async Task<ApiBaseResponse> ChangeApplicationStatus(ApplicationStatusCommand command)
        {
            if(!Enum.IsDefined(typeof(JobApplicationStatus), command.NewStatus))
            {
                return new BadRequestResponse("Invalid application status");
            }

            var application = await _repository.Application
                .GetAsync(ap => ap.Id == command.ApplicationId, true);
            if(application == null)
            {
                return new NotFoundResponse("Application record not found");
            }

            if(command.NewStatus == JobApplicationStatus.Applied 
                || command.NewStatus == JobApplicationStatus.Withdrawn || 
                application.Status > command.NewStatus)
            {
                return new BadRequestResponse($"You can not change the application status to {command.NewStatus.GetDescription()}");
            }

            application.Status = command.NewStatus;
            application.ModifiedAt = DateTime.UtcNow;
            await _repository.Application.EditAsync(application);

            switch (command.NewStatus)
            {
                case JobApplicationStatus.Interviewing:
                    // TODO: Send interview invite to the user
                    break;
                case JobApplicationStatus.OfferExtended:
                    //TODO: Send notice of offer to the Applicant
                    break;
                case JobApplicationStatus.Rejected:
                    //TODO: Send rejection mail to talent
                    break;
            }

            return new OkResponse<string>($"Application status changed to {command.NewStatus.GetDescription()}");
        }

        #region Private Methods
        private async Task RecalculateForJobAsync(Guid jobId)
        {
            var job = await _repository.Job
                .GetAsync(j => j.Id == jobId && !j.IsDeprecated);

            if (job == null) return;

            var applications = await _repository.Application
                .AsQueryable(a => a.JobId == job.Id)
                .ToListAsync();

            foreach (var app in applications)
            {
                BackgroundJob.Enqueue<IEligibilityService>(hf
                    => hf.EvaluateEligibility(job.Id, app.TalentId, 0.7));
            }
        }

        private async Task UpdateJobRequirements(Guid jobId, List<string> requirements)
        {
            var existingRequirements = await _repository.JobRequirement
                .GetAsync(jr => jr.JobId == jobId);

            var newRequirements = GetRequirements(jobId, requirements);

            await _repository.JobRequirement
                .AddRangeAsync(newRequirements, false);

            await _repository.JobRequirement
                .DeleteManyAsync(existingRequirements);

            await _repository.SaveAsync();
        }

        private async Task<List<JobSkill>> PopulateJobSkills(Guid jobId, List<string> skills, bool isForNewJob)
        {
            var normalizedNames = skills.Select(n => n.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingSkills = await _repository.Skill
                .AsQueryable(s => normalizedNames.Contains(s.Name))
                .ToListAsync();

            var missingNames = normalizedNames
                .Except(existingSkills.Select(s => s.Name), StringComparer.OrdinalIgnoreCase)
                .ToList();

            var newSkills = missingNames.Select(name => new Skill
            {
                Name = name.CapitalizeFirstLetterOnly(),
            }).ToList();

            await _repository.Skill.AddRangeAsync(newSkills);

            var allSkills = existingSkills.Concat(newSkills).ToList();

            var jobSkills = isForNewJob ? AddNewJobSkills(jobId, allSkills) :
                await UpdateJobSkills(jobId, allSkills);

            if(!isForNewJob && missingNames.Any())
            {
                await RecalculateForJobAsync(jobId);
            }

            return jobSkills;
        }

        private List<JobSkill> AddNewJobSkills(Guid jobId, List<Skill> skills)
        {
            var data = new List<JobSkill>();
            foreach (var skill in skills.DistinctBy(s => s.Id))
            {
                data.Add(PopulateSkill(jobId, skill.Id));
            }

            return data;
        }

        private async Task<List<JobSkill>> UpdateJobSkills(Guid jobId, List<Skill> skills)
        {
            var existingSkills = await _repository.JobSkill
                .GetAsync(s => s.JobId == jobId);

            if(existingSkills != null && existingSkills.Count > 0)
            {
                await _repository.JobSkill.RemoveManyAsync(existingSkills, false);
            }

            var data = new List<JobSkill>();
            foreach (var skill in skills)
            {
                data.Add(PopulateSkill(jobId, skill.Id));
            }

            return data;
        }

        private JobSkill PopulateSkill(Guid jobId, Guid skillId)
        {
            return new JobSkill
            {
                JobId = jobId,
                SkillId = skillId
            };
        }

        private List<JobRequirement> GetRequirements(Guid jobId, List<string> requirements)
        {
            var data = new List<JobRequirement>();
            foreach (var requirement in requirements)
            {
                data.Add(PopulateRequirement(jobId, requirement));
            }

            return data;
        }

        private JobRequirement PopulateRequirement(Guid jobId, string requirement)
        {
            return new JobRequirement
            {
                JobId = jobId,
                Requirement = requirement
            };
        }

        #endregion
    }
}