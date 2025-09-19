using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;

namespace PathFinder.Infrastructure.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _dbContext;
        private readonly Lazy<ITokenRepository> _tokenRepository;

        public RepositoryManager(AppDbContext dbContext) 
        {
            _dbContext = dbContext;

            _tokenRepository = new Lazy<ITokenRepository>(()
                => new TokenRepository(dbContext));
        }

        public ITokenRepository Token => _tokenRepository.Value;
        public async Task SaveAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}