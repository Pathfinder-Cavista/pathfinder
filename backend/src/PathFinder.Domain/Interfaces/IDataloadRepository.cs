using PathFinder.Domain.Entities;

namespace PathFinder.Domain.Interfaces
{
    public interface IDataloadRepository
    {
        Task AddDataAsync(List<AppUser> users, List<Job> jobs, List<JobApplication> applications);
    }
}