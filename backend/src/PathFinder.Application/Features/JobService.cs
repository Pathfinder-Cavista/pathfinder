using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathFinder.Application.Commands.Jobs;
using PathFinder.Application.DTOs;
using PathFinder.Application.Helpers;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Mappers;
using PathFinder.Application.Responses;
using PathFinder.Application.Validations.Jobs;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;
using PathFinder.Domain.Interfaces;

namespace PathFinder.Application.Features
{
    public class JobService : IJobService
    {
        private readonly IRepositoryManager _repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public JobService(IRepositoryManager repository,
                          UserManager<AppUser> userManager,
                          IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
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

        #region Private Methods
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

            return isForNewJob ? AddNewJobSkills(jobId, allSkills) :
                await UpdateJobSkills(jobId, allSkills);
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