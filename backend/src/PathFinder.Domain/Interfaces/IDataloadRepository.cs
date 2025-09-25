using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface IDataloadRepository
    {
        Task AddDataAsync(List<AppUser> users, List<Job> jobs, List<JobApplication> applications);
        Task AddReportAsync(Report report);
        Task EditReportAsync(Report report);
        Task<Report?> GetReportAsync(Expression<Func<Report, bool>> expression);
        Task<List<Report>> GetReportsAsync(Expression<Func<Report, bool>> expression);
    }
}