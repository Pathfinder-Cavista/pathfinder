using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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
                BackgroundJob.Enqueue(() => RunMetricsReportAsync(report.Id, currentYear, null!));
                return new OkResponse<string>($"Analytics report generation started. You'll be notified once completed");
            }
            catch (Exception)
            {
                //TODO: Log
                return new BadRequestResponse("Unable to generate the requested report. Please try again shortly");
            }
        }

        public async Task RunMetricsReportAsync(Guid reportId, int year, PerformContext context)
        {
            var report = await _repository.Dataload.GetReportAsync(r => r.Id == reportId) ?? 
                throw new ArgumentNullException(nameof(Report));
            
            try
            {
                ExcelPackage.License.SetNonCommercialOrganization("Axxess");
                
                using var package = new ExcelPackage();
                await RunApplicationTrendReportAsync(package, year);
                
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
        private async Task RunApplicationTrendReportAsync(ExcelPackage package, int year)
        {
            var records = await GetApplicationOvertimeAsync(year);
            var sheet = package.Workbook.Worksheets.Add("Application Trend");

            int headerRow = 1;
            int dataRowStart = 2;
            //Headers
            var headers = new string[] { "Month", "No. of Applications" };
            for (int i = 1; i <= headers.Length; i++)
            {
                sheet.Cells[headerRow, i].Value = headers[i - 1];
            }
            
            using (var range = sheet.Cells[headerRow, 1, headerRow, 2])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                range.Style.Font.Color.SetColor(Color.Black);
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            var row = dataRowStart;
            foreach(var record in records)
            {
                sheet.Cells[row, 1].Value = record.Month;
                sheet.Cells[row, 2].Value = record.Applications;
                row++;
            }

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

            //// Add Charts
            var chart = sheet.Drawings.AddChart("ApplicationTrendChart", eChartType.ColumnClustered) as ExcelBarChart;
            if(chart != null)
            {
                chart.Title.Text = $"Application Trend {year}";
                chart.SetPosition(dataRowStart - 1, 0, 3, 0);
                chart.SetSize(600, 400);

                var nameRange = sheet.Cells[dataRowStart, 1, dataRowStart + records.Count - 1, 1];
                var valueRange = sheet.Cells[dataRowStart, 2, dataRowStart + records.Count - 1, 2];

                chart.Series.Add(valueRange, nameRange);
                chart.Series[0].Header = "Count";
                chart.Legend.Remove();

                chart.DataLabel.ShowValue = true;
                chart.YAxis.Title.Text = "Number of Candidates";
            }

        }
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
