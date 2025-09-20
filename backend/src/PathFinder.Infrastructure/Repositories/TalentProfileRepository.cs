using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public class TalentProfileRepository : Repository<TalentProfile>, ITalentProfileRepository
    {
        public TalentProfileRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task AddAsync(TalentProfile profile, bool saveNow = true)
        {
            await InsertAsync(profile, saveNow);
        }

        public async Task EditAsync(TalentProfile profile, bool saveNow = true)
        {
            await UpdateAsync(profile, saveNow);
        }

        public async Task<TalentProfile?> GetAsync(Expression<Func<TalentProfile, bool>> expression, bool track = true)
        {
            return await FindOneAsync(expression, track);
        }
    }
}
