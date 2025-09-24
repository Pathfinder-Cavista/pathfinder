using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathFinder.Application.DTOs;
using PathFinder.Application.Helpers;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Mappers;
using PathFinder.Application.Responses;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;
using PathFinder.Domain.Interfaces;
using System.Security.Claims;

namespace PathFinder.Application.Features
{
    public class DataloadService : IDataloadService
    {
        private readonly ClaimsPrincipal? _principal;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEligibilityService _eligibility;
        private readonly IRepositoryManager _repository;
        private readonly PasswordHasher<AppUser> _passwordHasher;
        private const string _pass = "P@55w0rd";
        private const string dummy_cv = "https://res.cloudinary.com/otrprojs/raw/upload/v1758646102/documents/f9091e58f7554877848462614152576d.pdf";

        public DataloadService(UserManager<AppUser> userManager,
                               IHttpContextAccessor contextAccessor,
                               IEligibilityService eligibility,
                               IRepositoryManager repository)
        {
            _userManager = userManager;
            _principal = contextAccessor.HttpContext?.User;
            _eligibility = eligibility;
            _repository = repository;
            _passwordHasher = new PasswordHasher<AppUser>();
        }

        public async Task<ApiBaseResponse> RunDataloadAsync(IFormFile file)
        {
            var userId = AccountHelpers.GetLoggedInUserId(_principal);
            if (string.IsNullOrEmpty(userId))
            {
                return new ForbiddenResponse("Access denied!!!");
            }

            var user = await _userManager.Users
                .Include(u => u.Recruiter)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null || user.Recruiter == null)
            {
                return new NotFoundResponse("User not found");
            }

            var fileValidation = file.IsAValidFile(UploadMediaType.Document);
            if (!fileValidation.Valid)
            {
                return new BadRequestResponse(fileValidation.Message);
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!fileExtension.Equals(".xls") && !fileExtension.Equals(".xlsx"))
            {
                return new BadRequestResponse("Invalid file type. File should have .xls or .xlsx extensions.");
            }

            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var filePath = Path.Combine(uploadDirectory, Guid.NewGuid() + fileExtension);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var jobId = BackgroundJob.Enqueue(() => LoadDataAsync(filePath, user.Recruiter.Id, null!));
            return new OkResponse<string>($"Dataload started. Background job id: {jobId}");
        }

        public async Task LoadDataAsync(string filePath, Guid recruiterId, PerformContext context)
        {
            context.WriteLine($"Reading file content from path: {filePath}");
            byte[] bytes = File.ReadAllBytes(filePath);
            using var stream = new MemoryStream(bytes);
            if(stream.Length <= 0)
            {
                context.WriteLine("Invalid stream length.");
                return;
            }

            context.WriteLine("Loading data from the file");
            var (Jobs, Users, Applications) = stream.LoadDataloadSeedData();
            context.WriteLine($"Data loaded from the file: Jobs: {Jobs.Count}, Users: {Users.Count}, Applications: {Applications.Count}");

            //Clean up the file created to the disc
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                context.WriteLine("File deleted successfully");
            }

            var skillsToUpdate = new List<string>();
            foreach(var job in Jobs)
            {
                skillsToUpdate.AddRange(job.Skills);
            }
            
            foreach(var user in Users)
            {
                skillsToUpdate.AddRange(user.ProfileSkills);
            }

            context.WriteLine($"Adding/Updating {skillsToUpdate.Count} skills...");
            var allSkills = await _eligibility.HandleSkillsUpdate(skillsToUpdate);
            context.WriteLine($"Added/Updated {allSkills.Count} skills...");

            context.WriteLine("Mapping dataload jobs to Jobs");
            var jobsTuple = MapJobs(Jobs, allSkills, recruiterId);
            context.WriteLine($"Mapped {jobsTuple.Count} jobs");

            context.WriteLine("Mapping dataload users to AppUser");
            var usersTuple = MapUsers(Users, allSkills);
            context.WriteLine($"Mapped {usersTuple.Count} users");

            context.WriteLine("Mapping dataload applications to app applications");
            var applications = MapApplications(Applications, jobsTuple, usersTuple);
            context.WriteLine($"Mapped {applications.Count} applications");

            context.WriteLine("Writing the data to the Database...");
            await _repository.Dataload.AddDataAsync([.. usersTuple.Select(ut => ut.User)],
                [.. jobsTuple.Select(jt => jt.Job)], applications);
            context.WriteLine("Successfully wrote the data to the Database.");
        }

        #region Private Method
        private List<JobApplication> MapApplications(List<DataloadJobApplication> applications, 
                                                     List<(long JobId, Job Job)> jobs, 
                                                     List<(long UserId, AppUser User)> users)
        {
            var result = new List<JobApplication>();
            foreach (var application in applications)
            {
                var job = jobs.FirstOrDefault(j => j.JobId == application.JobId);
                var user = users.FirstOrDefault(u => u.UserId == application.UserId);
                if(job != default && user != default && user.User.Talent != null)
                {
                    var (Eligible, Score) = EvaluateEligibility(application, [.. job.Job.RequiredSkills], [.. user.User.Talent.Skills]);
                    result.Add(new JobApplication
                    {
                        ResumeUrl = user.User.Talent.ResumeUrl,
                        JobId = job.Job.Id,
                        TalentId = user.User.Talent.Id,
                        CreatedAt = application.ApplicationDate.ToDayEnd(),
                        Status = application.Status,
                        IsEligible = Eligible,
                        AttainedThreshold = Score
                    });
                }
            }

            return result;
        }

        public List<(long JobId, Job Job)> MapJobs(List<DataloadJob> dataloadJobs, List<Skill> skills, Guid recruiterId)
        {
            var result = new List<(long JobId, Job Job)>();
            foreach(var job in dataloadJobs)
            {
                var jobSkills = skills.Where(s => job.Skills
                    .Contains(s.Name, StringComparer.OrdinalIgnoreCase))
                    .ToList();

                var newJob = JobCommandMapper.ToJobModel(job, recruiterId);
                newJob.RequiredSkills = [.. jobSkills.Select(s => new JobSkill
                {
                    JobId = newJob.Id,
                    SkillId = s.Id
                })];
                newJob.Requirements = [.. job.Requirements.Select(r => new JobRequirement
                {
                    JobId = newJob.Id,
                    Requirement = r
                })];

                result.Add((job.Id, newJob));
            }

            return result;
        }

        private List<(long UserId, AppUser User)> MapUsers(List<DataloadUserProfile> dataloadUsers, List<Skill> skills)
        {
            var result = new List<(long UserId, AppUser User)>();
            foreach (var user in dataloadUsers)
            {
                var userSkills = skills.Where(s => user.ProfileSkills
                    .Contains(s.Name, StringComparer.OrdinalIgnoreCase))
                    .ToList();
                var newUser = UserCommandMapper.ToAppUser(user, dummy_cv);
                if(newUser != null && newUser.Talent != null)
                {
                    newUser.PasswordHash = _passwordHasher.HashPassword(newUser, _pass);
                    newUser.Talent.Skills = [.. userSkills.Select(s => new TalentSkill
                    {
                        SkillId = s.Id,
                        TalentProfileId = newUser.Talent.Id
                    })];
                    result.Add((user.Id, newUser));
                }
            }

            return result;
        }

        private (bool Eligible, double Score) EvaluateEligibility(DataloadJobApplication application,
                                                                  List<JobSkill> jobSkills,
                                                                  List<TalentSkill> talentSkills,
                                                                  double threshold = 0.7)
        {
            var eligible = (application.Status >= JobApplicationStatus.Interviewing && application.Status != JobApplicationStatus.Rejected);
            var score = eligible ? threshold : 0.0;

            var matches = jobSkills.Count(s
                => talentSkills.Select(ts => ts.SkillId).Contains(s.SkillId));

            var percentage = (double)matches / jobSkills.Count;
            percentage = percentage > score ? percentage : score;
            eligible = percentage >= threshold;
            score = percentage;
            return (eligible, score);
        }
        #endregion
    }
}