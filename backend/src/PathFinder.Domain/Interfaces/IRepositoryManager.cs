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
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
