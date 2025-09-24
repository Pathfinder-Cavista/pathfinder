using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface IHolidayRepository
    {
        Task AddAsync(Holiday holiday, bool save = true);
        Task AddRangeAsync(List<Holiday> holidays, bool save = true);
        IQueryable<Holiday> AsQueryable(Expression<Func<Holiday, bool>> expression);
        Task DeleteOneAsync(Holiday holiday, bool save = true);
        Task EditAsync(Holiday holiday, bool save = true);
        Task<Holiday?> GetOneAsync(Expression<Func<Holiday, bool>> expression, bool track = false);
    }
}