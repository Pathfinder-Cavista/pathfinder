namespace PathFinder.Domain.Entities
{
    public class JobSkill : BaseEntity
    {
        public Guid JobId { get; set; }
        public Job? Job { get; set; }

        public Guid SkillId { get; set; }
        public Skill? Skill { get; set; }

        public bool IsRequired { get; set; }
    }
}
