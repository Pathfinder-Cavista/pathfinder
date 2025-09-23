using Microsoft.EntityFrameworkCore;
using PathFinder.Application.Helpers;
using PathFinder.Application.Interfaces;
using PathFinder.Domain.Interfaces;

namespace PathFinder.Application.Features
{
    public class EligibilityService : IEligibilityService
    {
        private readonly IRepositoryManager _repository;

        public EligibilityService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task EvaluateEligibility(Guid jobId, Guid talentId, double threshold = 0.7)
        {
            var talent = await _repository.TalentProfile
                .AsQueryable(talent => talent.Id == talentId)
                .Include(t => t.Skills)
                .Include(t => t.Applications)
                .FirstOrDefaultAsync();
            if (talent == null || !talent.Applications.IsNotNullOrEmpty()) 
            {
                return;
            }

            var application = talent.Applications
                .FirstOrDefault(ap => ap.JobId == jobId && ap.TalentId == talentId);
            if (application == null)
            {
                return;
            }

            var job = await _repository.Job
                .GetQueryable(j => j.Id == jobId)
                .Include(j => j.RequiredSkills)
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return;
            }

            if (job.RequiredSkills.IsNullOrEmpty())
            {
                application.IsEligible = true;
                application.AttainedThreshold = threshold;
                await _repository.Application
                    .EditAsync(application);
                return;
            }

            var total = job.RequiredSkills.Count;
            var jobSkillIds = job.RequiredSkills.Select(sk => sk.SkillId);
            var talentSkillIds = talent.Skills.Select(sk => sk.SkillId).ToList();

            var jobSkills = await _repository.Skill
                .AsQueryable(s => jobSkillIds.Contains(s.Id))
                .Select(s => s.Name)
                .ToListAsync();

            var talentSkills = await _repository.Skill
                .AsQueryable(s => talentSkillIds.Contains(s.Id))
                .Select(s => s.Name)
                .ToListAsync();

            var matches = jobSkills.Count(s 
                => talentSkills.Contains(s, StringComparer.OrdinalIgnoreCase));

            var percentage = (double)matches / total;
            application.IsEligible = percentage >= threshold;
            application.AttainedThreshold = percentage;
            await _repository.Application.EditAsync(application);
        }
    }
}
