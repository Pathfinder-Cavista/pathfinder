using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    internal class RecruiterProfileRepository : Repository<RecruiterProfile>, IRecruiterProfileRepository
    {
        public RecruiterProfileRepository(AppDbContext context) : base(context)
        {

        }

        public async Task AddAsync(RecruiterProfile profile, bool saveNow = true)
        {
            await InsertAsync(profile, saveNow);
        }

        public async Task EditAsync(RecruiterProfile profile, bool saveNow = true)
        {
            await UpdateAsync(profile, saveNow);
        }

        public async Task<RecruiterProfile?> GetAsync(Expression<Func<RecruiterProfile, bool>> expression, bool track = true)
        {
            return await FindOneAsync(expression, track);
        }
    }
}
