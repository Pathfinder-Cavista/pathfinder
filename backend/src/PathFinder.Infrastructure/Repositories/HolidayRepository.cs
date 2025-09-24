using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(AppDbContext context) : base(context) { }

        public async Task AddAsync(Holiday holiday, bool save = true)
        {
            await InsertAsync(holiday, save);
        }

        public async Task AddRangeAsync(List<Holiday> holidays, bool save = true)
        {
            await InsertRangeAsync(holidays, save);
        }

        public IQueryable<Holiday> AsQueryable(Expression<Func<Holiday, bool>> expression)
        {
            return GetAsQueryable(expression);
        }

        public async Task<Holiday?> GetOneAsync(Expression<Func<Holiday, bool>> expression, bool track = false)
        {
            return await FindOneAsync(expression, track);
        }

        public async Task EditAsync(Holiday holiday, bool save = true)
        {
            await UpdateAsync(holiday, save);
        }

        public async Task DeleteOneAsync(Holiday holiday, bool save = true)
        {
            await DeleteAsync(holiday, save);
        }
    }
}