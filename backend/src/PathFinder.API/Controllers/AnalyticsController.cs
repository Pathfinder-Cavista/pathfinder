using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/analytics")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AnalyticsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("applications-per-job")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ApplicationPerJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsPerJob()
        {
            return Ok(await _service.Analytics.GetApplicationsPerJobAsync());
        }

        [HttpGet("applications-by-location")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ApplicationPerJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsByLocation()
        {
            return Ok(await _service.Analytics.GetApplicationsByLocationAsync());
        }

        [HttpGet("application-trend")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ApplicationPerJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsOvertime()
        {
            return Ok(await _service.Analytics.GetApplicationOvertimeAsync());
        }

        [HttpGet("hire-rate-by-jobtype")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ApplicationPerJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> HireRateByJobType()
        {
            return Ok(await _service.Analytics.GetHireRateByJobTypeAsync());
        }

        [HttpGet("applications-by-status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ApplicationPerJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplicationsByStatus()
        {
            return Ok(await _service.Analytics.GetApplicationStatusDistributionAsync());
        }

        [HttpGet("jobstatus-distribution")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ApplicationPerJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> JobStatusDistribution()
        {
            return Ok(await _service.Analytics.GetJobStatusDistributionAsync());
        }
    }
}