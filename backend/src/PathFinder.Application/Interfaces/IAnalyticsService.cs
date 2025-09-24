using PathFinder.Application.DTOs;

namespace PathFinder.Application.Interfaces
{
    public interface IAnalyticsService
    {
        Task<List<ApplicationsOvertimeDto>> GetApplicationOvertimeAsync();
        Task<List<ApplicationsByLocationDto>> GetApplicationsByLocationAsync();
        Task<List<ApplicationPerJobDto>> GetApplicationsPerJobAsync();
        Task<List<ApplicationStatusDistributionDto>> GetApplicationStatusDistributionAsync();
        Task<List<HireRateByJobTypeDto>> GetHireRateByJobTypeAsync();
        Task<List<JobStatusDistributionDto>> GetJobStatusDistributionAsync();
    }
}
