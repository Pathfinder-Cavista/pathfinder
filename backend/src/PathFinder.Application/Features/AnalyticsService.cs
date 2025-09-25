using ClosedXML.Excel;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using PathFinder.Application.DTOs;
using PathFinder.Application.Helpers;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Responses;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;
using PathFinder.Domain.Interfaces;
using System.Drawing;
using System.Globalization;
using System.Security.Claims;

namespace PathFinder.Application.Features
{
    public class AnalyticsService(IRepositoryManager repository, 
        IHttpContextAccessor contextAccessor, 
        UserManager<AppUser> userManager,
        IUploadService uploadService) : IAnalyticsService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ClaimsPrincipal? _principal = contextAccessor.HttpContext?.User;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IUploadService _uploadService = uploadService;

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

        public async Task<ApiBaseResponse> FetchCompletedReports()
        {
            var reports = await _repository.Dataload.GetReportsAsync(r => !r.IsDeprecated);
            return new OkResponse<List<ReportDto>>(reports.Select(MapToDto).ToList());
        }

        public async Task<ApiBaseResponse> GenerateMetricsReportAsync(int currentYear)
        {
            try
            {

                var userId = AccountHelpers.GetLoggedInUserId(_principal);
                AppUser? user = null;
                if (userId != null)
                {
                    user = await _userManager.FindByIdAsync(userId);
                }

                var report = new Report
                {
                    Name = "Analytics Reports",
                    UserName = user != null ? $"{user.FirstName} {user.LastName}" : "System",
                };

                await _repository.Dataload.AddReportAsync(report);
                BackgroundJob.Enqueue(() => RunMetricsReportAsync(report.Id, null!));
                return new OkResponse<string>($"Analytics report generation started. You'll be notified once completed");
            }
            catch (Exception)
            {
                //TODO: Log
                return new BadRequestResponse("Unable to generate the requested report. Please try again shortly");
            }
        }

        public async Task RunMetricsReportAsync(Guid reportId, PerformContext context)
        {
            var report = await _repository.Dataload.GetReportAsync(r => r.Id == reportId) ?? 
                throw new ArgumentNullException(nameof(Report));
            
            try
            {
                ExcelPackage.License.SetNonCommercialOrganization("Axxess");
                var year = DateTime.Now.Year;
                var appByLoc = await GetApplicationOvertimeAsync(year);

                using var package = new ExcelPackage();
                var sheet = package.Workbook.Worksheets.Add("ApplicationTrend");

                //Headers
                sheet.Cells[1, 1].Value = "Month";
                sheet.Cells[1, 2].Value = "No. of Applications";
                using (var range = sheet.Cells[1, 1, 1, 2])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                    range.Style.Font.Color.SetColor(Color.Black);
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                for (int i = 0; i < appByLoc.Count; i++)
                {
                    sheet.Cells[i + 2, 1].Value = appByLoc[i].Month;
                    sheet.Cells[i + 2, 2].Value = appByLoc[i].Applications;
                }

                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                // Add Charts
                var chart = sheet.Drawings.AddChart("ApplicationChart", eChartType.ColumnClustered);
                chart.Title.Text = $"Application Trend {year}";
                chart.SetPosition(1, 0, 3, 0);
                chart.SetSize(600, 400);

                var series = chart.Series.Add(sheet.Cells[2, 2, appByLoc.Count + 1, 2],
                                              sheet.Cells[2, 1, appByLoc.Count + 1, 1]);

                series.Header = "Applications";

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);
                stream.Position = 0;

                var reportName = string.Format("{0}-{1}", report.Name.Replace(" ", ""), DateTime.Now.Ticks);
                var uploadResult = await _uploadService.UploadRawAsync(reportName, $"{reportName}.xlsx", stream);
                if(uploadResult !=  null && !string.IsNullOrEmpty(uploadResult.Url))
                {
                    report.Status = ReportStatus.Completed;
                    report.IsComplete = true;
                    report.CompletionTime = DateTime.UtcNow;
                    report.AssetUrl = uploadResult.Url;

                    await _repository.Dataload.EditReportAsync(report);
                }
                //return stream;
            }
            catch (Exception)
            {
                if(report != null)
                {
                    report.Status = ReportStatus.Failed;
                    report.ModifiedAt = DateTime.UtcNow;
                    report.CompletionTime = DateTime.UtcNow;
                    report.IsComplete = true;

                    await _repository.Dataload.EditReportAsync(report);
                }
                throw;
            }
        }
        #region Private Methods
        private ReportDto MapToDto(Report report)
        {
            return new ReportDto
            {
                Id = report.Id,
                Name = report.Name,
                UserName = report.UserName,
                AssetUrl = report.AssetUrl,
                StartTime = report.CreatedAt,
                IsComplete = report.IsComplete,
                Status = report.Status.GetDescription(),
                CompleteTime = report.CompletionTime,
            };
        }

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
