namespace PathFinder.Domain.Entities
{
    public class TalentSkill : BaseEntity
    {
        public Guid TalentProfileId { get; set; }
        public TalentProfile? TalentProfile { get; set; }

        public Guid SkillId { get; set; }
        public Skill? Skill { get; set; }
    }
}
