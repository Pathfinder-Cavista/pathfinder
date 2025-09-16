using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;

namespace PathFinder.Infrastructure.Repositories
{
    public sealed class SampleRepository : Repository<Sample>, ISampleRepository
    {
        public SampleRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task AddAsync(Sample sample)
        {
            await InsertAsync(sample, false);
        }
    }
}
