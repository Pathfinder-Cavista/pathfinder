using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;

namespace PathFinder.Infrastructure.Repositories
{
    public class DataloadRepository : IDataloadRepository
    {
        private readonly AppDbContext _context;

        public DataloadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddDataAsync(List<AppUser> users,
                                       List<Job> jobs,
                                       List<JobApplication> applications)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Jobs.AddRange(jobs);
                _context.Users.AddRange(users);
                _context.Applications.AddRange(applications);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}