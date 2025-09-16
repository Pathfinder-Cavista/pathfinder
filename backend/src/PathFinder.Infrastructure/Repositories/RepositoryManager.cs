using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;

namespace PathFinder.Infrastructure.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _dbContext;
        private readonly Lazy<ISampleRepository> _sampleRepository;

        public RepositoryManager(AppDbContext dbContext) 
        {
            _dbContext = dbContext;

            _sampleRepository = new Lazy<ISampleRepository>(()
                => new SampleRepository(dbContext));
        }

        public ISampleRepository Sample => _sampleRepository.Value;
        public async Task SaveAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}