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
        private readonly Lazy<IJobRepository> _jobRepository;
        private readonly Lazy<IJobRequirementRepository> _jobRequirementRepository;
        private readonly Lazy<IOrganizationRepository> _organizationRepository;
        private readonly Lazy<IJobApplicationRepository> _jobApplicationRepository;

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

            _jobRepository = new Lazy<IJobRepository>(()
                => new JobRepository(dbContext));

            _jobRequirementRepository = new Lazy<IJobRequirementRepository>(()
                => new JobRequirementRepository(dbContext));

            _organizationRepository = new Lazy<IOrganizationRepository>(()
                => new OrganizationRepository(dbContext));

            _jobApplicationRepository = new Lazy<IJobApplicationRepository>(()
                => new JobApplicationRepository(dbContext));
        }

        public ITokenRepository Token => _tokenRepository.Value;
        public ISkillRepository Skill => _skillRepository.Value;
        public ITalentProfileRepository TalentProfile => _talentProfileRepository.Value;
        public IRecruiterProfileRepository RecruiterProfile => _recruiterProfileRepository.Value;
        public ITalentSkillRepository TalentSkill => _talentSkillRepository.Value;
        public IJobSkillRepository JobSkill => _jobSkillRepository.Value;
        public IJobRepository Job => _jobRepository.Value;
        public IJobRequirementRepository JobRequirement => _jobRequirementRepository.Value;
        public IOrganizationRepository Organization => _organizationRepository.Value;
        public IJobApplicationRepository Application => _jobApplicationRepository.Value;

        public async Task SaveAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}