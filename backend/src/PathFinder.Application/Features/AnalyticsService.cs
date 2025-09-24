using Microsoft.EntityFrameworkCore;
using PathFinder.Application.DTOs;
using PathFinder.Application.Helpers;
using PathFinder.Application.Interfaces;
using PathFinder.Domain.Enums;
using PathFinder.Domain.Interfaces;
using System.Globalization;

namespace PathFinder.Application.Features
{
    public class AnalyticsService(IRepositoryManager repository) : IAnalyticsService
    {
        private readonly IRepositoryManager _repository = repository;

        public async Task<double> GetAverageTimeToFillAsync()
        {
            var durations = await _repository.Job
                .GetQueryable(j => !j.IsDeprecated && j.Status == JobStatus.Closed && j.ClosingDate.HasValue)
                .Select(j => new { j.CreatedAt, j.ClosingDate })
                .ToListAsync();

            return durations
                .Average(j => (j.ClosingDate!.Value - j.CreatedAt).TotalDays);
        }

        public async Task<List<OpenRoleDurationDto>> GetOpenRoleDurationAsync()
        {
            var holidays = (await _repository.Holiday
                .AsQueryable(h => !h.IsDeprecated)
                .ToListAsync())
                .Select(h => h.IsRecurring ? 
                    new DateTime(DateTime.Today.Year, h.Date.Month, h.Date.Day) : 
                    h.Date.Date).ToHashSet();

            var jobs = await _repository.Job
                .GetQueryable(j => !j.IsDeprecated && (j.Status == JobStatus.Published || j.Status == JobStatus.Closed))
                .Select(j => new { j.Id, j.Title, j.CreatedAt, j.ClosingDate, j.Status })
                .ToListAsync();

            var result = jobs.Select(j => new OpenRoleDurationDto
            {
                RoleId = j.Id,
                RoleTitle = j.Title,
                Status = j.Status,
                StatusText = j.Status.GetDescription(),
                OpenDays = GetBusinessDays(j.CreatedAt, j.ClosingDate ?? DateTime.Today, holidays)
            }).ToList();

            return result;
        }

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

        public async Task<List<YearlyApplicationTrendsDto>> GetApplicationOvertimeAsync(int year)
        {
            year = year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year ? DateTime.UtcNow.Year : year;
            var monthlyTrend = await _repository.Application
                .AsQueryable(a => !a.IsDeprecated && a.CreatedAt.Year == year)
                .ToListAsync();

            var data = monthlyTrend
                .GroupBy(a => a.CreatedAt.Month)
                .Select(grp => new YearlyApplicationTrendsDto
                {
                    MonthKey = grp.Key,
                    Applications = grp.Count()
                }).OrderBy(a => a.Month)
                .ToList();

            var fullTrend = Enumerable.Range(1, 12)
                .GroupJoin(data, m => m, t => t.MonthKey,
                    (m, t) => new YearlyApplicationTrendsDto
                    {
                        MonthKey = m,
                        Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m),
                        Applications = t.FirstOrDefault()?.Applications ?? 0
                    }).OrderBy(a => a.MonthKey);

            return [.. fullTrend];
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

        #region Private Methods
        private static int GetBusinessDays(DateTime start, DateTime end, HashSet<DateTime> holidays)
        {
            int businessDays = 0;
            for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
            {
                if(date.DayOfWeek is DayOfWeek.Saturday || date.DayOfWeek is DayOfWeek.Sunday)
                {
                    continue;
                }

                if (holidays.Contains(date.Date))
                {
                    continue;
                }

                businessDays++;
            }
            return businessDays;
        }
        #endregion
    }
}
