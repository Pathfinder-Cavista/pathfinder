using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Filters;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/analytics")]
    [ApiController]
    [AnalyticsPermission]
    public class AnalyticsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AnalyticsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("open-roles-durations")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<OpenRoleDurationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOpenRolesDurations()
        {
            return Ok(await _service.Analytics.GetOpenRoleDurationAsync());
        }

        [HttpGet("average-time-to-fill")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AverageTimeToFill()
        {
            return Ok(await _service.Analytics.GetAverageTimeToFillAsync());
        }

        [HttpGet("applications-per-job")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ApplicationPerJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsPerJob()
        {
            return Ok(await _service.Analytics.GetApplicationsPerJobAsync());
        }

        [HttpGet("applications-by-location")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ApplicationsByLocationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsByLocation()
        {
            return Ok(await _service.Analytics.GetApplicationsByLocationAsync());
        }

        [HttpGet("application-trend")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ApplicationsOvertimeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsOvertime()
        {
            return Ok(await _service.Analytics.GetApplicationOvertimeAsync());
        }

        [HttpGet("application-trend/{year:int}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<YearlyApplicationTrendsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> YearlyApplicationTrend([FromRoute] int year)
        {
            return Ok(await _service.Analytics.GetApplicationOvertimeAsync(year));
        }

        [HttpGet("hire-rate-by-jobtype")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<HireRateByJobTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> HireRateByJobType()
        {
            return Ok(await _service.Analytics.GetHireRateByJobTypeAsync());
        }

        [HttpGet("applications-by-status")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ApplicationStatusDistributionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsByStatus()
        {
            return Ok(await _service.Analytics.GetApplicationStatusDistributionAsync());
        }

        [HttpGet("jobstatus-distribution")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<JobStatusDistributionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> JobStatusDistribution()
        {
            return Ok(await _service.Analytics.GetJobStatusDistributionAsync());
        }
    }
}