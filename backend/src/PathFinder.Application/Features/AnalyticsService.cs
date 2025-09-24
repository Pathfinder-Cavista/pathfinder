using Microsoft.EntityFrameworkCore;
using PathFinder.Application.DTOs;
using PathFinder.Application.Helpers;
using PathFinder.Application.Interfaces;
using PathFinder.Domain.Enums;
using PathFinder.Domain.Interfaces;

namespace PathFinder.Application.Features
{
    public class AnalyticsService(IRepositoryManager repository) : IAnalyticsService
    {
        private readonly IRepositoryManager _repository = repository;

        public async Task<List<ApplicationPerJobDto>> GetApplicationsPerJobAsync()
        {
            return await _repository.Application
                .AsQueryable(_ => !_.IsDeprecated)
                .Include(a => a.Job)
                .GroupBy(a => a.JobId)
                .Select(g => new ApplicationPerJobDto
                {
                    JobId = g.Key,
                    JobTitle = g.First().Job!.Title,
                    Count = g.LongCount()
                }).OrderByDescending(a => a.Count)
                .ToListAsync();
        }

        public async Task<List<ApplicationsByLocationDto>> GetApplicationsByLocationAsync()
        {
            return await _repository.Application
                .AsQueryable(a => !a.IsDeprecated)
                .Include(a => a.Job)
                .GroupBy(a => a.Job!.Location)
                .Select(grp => new ApplicationsByLocationDto
                {
                    Location = grp.Key ?? "Remote",
                    Applications = grp.Count()
                }).ToListAsync();
        }

        public async Task<List<JobStatusDistributionDto>> GetJobStatusDistributionAsync()
        {
            return await _repository.Job
                .GetQueryable(j => !j.IsDeprecated)
                .GroupBy(j => j.Status)
                .Select(grp => new JobStatusDistributionDto
                {
                    Status = grp.Key,
                    StatusText = grp.Key.GetDescription(),
                    Count = grp.Count()
                }).ToListAsync();
        }

        public async Task<List<ApplicationStatusDistributionDto>> GetApplicationStatusDistributionAsync()
        {
            return await _repository.Application
                .AsQueryable(j => !j.IsDeprecated)
                .GroupBy(j => j.Status)
                .Select(grp => new ApplicationStatusDistributionDto
                {
                    Status = grp.Key,
                    StatusText = grp.Key.GetDescription(),
                    Count = grp.Count()
                }).ToListAsync();
        }

        public async Task<List<ApplicationsOvertimeDto>> GetApplicationOvertimeAsync()
        {
            return await _repository.Application
                .AsQueryable(a  => !a.IsDeprecated)
                .GroupBy(a => a.CreatedAt.Date)
                .Select(grp => new ApplicationsOvertimeDto
                {
                    Date = grp.Key,
                    Count = grp.Count()
                }).OrderBy(a => a.Date)
                .ToListAsync();
        }

        public async Task<List<HireRateByJobTypeDto>> GetHireRateByJobTypeAsync()
        {
            return await _repository.Application
                .AsQueryable(a => !a.IsDeprecated && a.Status == JobApplicationStatus.Hired)
                .Include(a => a.Job)
                .GroupBy(a => a.Job!.EmploymentType)
                .Select(grp => new HireRateByJobTypeDto
                {
                    JobType = grp.Key,
                    JobTypeText = grp.Key.GetDescription(),
                    Hires = grp.Count()
                }).ToListAsync();
        }
    }
}
