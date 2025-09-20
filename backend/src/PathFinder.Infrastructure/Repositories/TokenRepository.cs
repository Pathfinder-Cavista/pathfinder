using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public sealed class TokenRepository : Repository<RefreshToken>, ITokenRepository
    {
        public TokenRepository(AppDbContext dbContext) 
            : base(dbContext) { }

        public async Task AddAsync(RefreshToken token)
        {
            await InsertAsync(token, false);
        }

        public async Task<RefreshToken?> GetAsync(Expression<Func<RefreshToken, bool>> predicate, bool track = false)
        {
            return await FindOneAsync(predicate, track);
        }
    }
}
