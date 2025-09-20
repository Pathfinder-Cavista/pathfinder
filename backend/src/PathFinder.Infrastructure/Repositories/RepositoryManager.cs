using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;

namespace PathFinder.Infrastructure.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _dbContext;
        private readonly Lazy<ITokenRepository> _tokenRepository;
        private readonly Lazy<ISkillRepository> _skillRepository;
        private readonly Lazy<ITalentProfileRepository> _talentProfileRepository;
        private readonly Lazy<IRecruiterProfileRepository> _recruiterProfileRepository;
        private readonly Lazy<ITalentSkillRepository> _talentSkillRepository;
        private readonly Lazy<IJobSkillRepository> _jobSkillRepository;


        public RepositoryManager(AppDbContext dbContext) 
        {
            _dbContext = dbContext;

            _tokenRepository = new Lazy<ITokenRepository>(()
                => new TokenRepository(dbContext));

            _skillRepository = new Lazy<ISkillRepository>(() 
                => new SkillRepository(dbContext));

            _talentProfileRepository = new Lazy<ITalentProfileRepository>(()
                => new TalentProfileRepository(dbContext));

            _recruiterProfileRepository = new Lazy<IRecruiterProfileRepository>(()
                => new RecruiterProfileRepository(dbContext));

            _talentSkillRepository = new Lazy<ITalentSkillRepository>(()
                => new TalentSkillRepository(dbContext));

            _jobSkillRepository = new Lazy<IJobSkillRepository>(()
                => new JobSkillRepository(dbContext));
        }

        public ITokenRepository Token => _tokenRepository.Value;
        public ISkillRepository Skill => _skillRepository.Value;
        public ITalentProfileRepository TalentProfile => _talentProfileRepository.Value;
        public IRecruiterProfileRepository RecruiterProfile => _recruiterProfileRepository.Value;
        public ITalentSkillRepository TalentSkill => _talentSkillRepository.Value;
        public IJobSkillRepository JobSkill => _jobSkillRepository.Value;

        public async Task SaveAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}