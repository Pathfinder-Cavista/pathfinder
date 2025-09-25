using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Controllers.Extensions;
using PathFinder.API.Filters;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/analytics")]
    [ApiController]
    [AnalyticsPermission]
    public class AnalyticsController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public AnalyticsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("open-roles-durations")]
        [ProducesResponseType(typeof(List<OpenRoleDurationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOpenRolesDurations()
        {
            return Ok(await _service.Analytics.GetOpenRoleDurationAsync());
        }

        [HttpGet("average-time-to-fill")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AverageTimeToFill()
        {
            return Ok(await _service.Analytics.GetAverageTimeToFillAsync());
        }

        [HttpGet("applications-per-job")]
        [ProducesResponseType(typeof(List<ApplicationPerJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsPerJob()
        {
            return Ok(await _service.Analytics.GetApplicationsPerJobAsync());
        }

        [HttpGet("applications-by-location")]
        [ProducesResponseType(typeof(List<ApplicationsByLocationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsByLocation()
        {
            return Ok(await _service.Analytics.GetApplicationsByLocationAsync());
        }

        [HttpGet("application-trend")]
        [ProducesResponseType(typeof(List<ApplicationsOvertimeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsOvertime()
        {
            return Ok(await _service.Analytics.GetApplicationOvertimeAsync());
        }

        [HttpGet("application-trend/{year:int}")]
        [ProducesResponseType(typeof(List<YearlyApplicationTrendsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> YearlyApplicationTrend([FromRoute] int year)
        {
            return Ok(await _service.Analytics.GetApplicationOvertimeAsync(year));
        }

        [HttpGet("hire-rate-by-jobtype")]
        [ProducesResponseType(typeof(List<HireRateByJobTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> HireRateByJobType()
        {
            return Ok(await _service.Analytics.GetHireRateByJobTypeAsync());
        }

        [HttpGet("applications-by-status")]
        [ProducesResponseType(typeof(List<ApplicationStatusDistributionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsByStatus()
        {
            return Ok(await _service.Analytics.GetApplicationStatusDistributionAsync());
        }

        [HttpGet("jobstatus-distribution")]
        [ProducesResponseType(typeof(List<JobStatusDistributionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> JobStatusDistribution()
        {
            return Ok(await _service.Analytics.GetJobStatusDistributionAsync());
        }

        [HttpGet("export")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportAnalyticsToExcels([FromQuery] int year)
        {
            year = year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year ? DateTime.Now.Year: year;
            var response = await _service.Analytics.GenerateMetricsReportAsync(year);
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<string>());
        }

        [HttpGet("export-completed")]
        [ProducesResponseType(typeof(List<ReportDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CompletedReports()
        {
            var response = await _service.Analytics.FetchCompletedReports();
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<List<ReportDto>>());
        }
    }
}