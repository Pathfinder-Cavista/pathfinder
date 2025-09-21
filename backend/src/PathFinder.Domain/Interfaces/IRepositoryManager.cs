namespace PathFinder.Domain.Interfaces
{
    public interface IRepositoryManager
    {
        ITokenRepository Token { get; }
        ISkillRepository Skill { get; }
        ITalentProfileRepository TalentProfile { get; }
        IRecruiterProfileRepository RecruiterProfile { get; }
        ITalentSkillRepository TalentSkill { get; }
        IJobSkillRepository JobSkill { get; }
        IJobRepository Job {  get; }
        IJobRequirementRepository JobRequirement { get; }
        IOrganizationRepository Organization { get; }
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
