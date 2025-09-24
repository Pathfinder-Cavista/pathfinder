using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Controllers.Extensions;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/dataload")]
    [ApiController]
    public class DataloadController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public DataloadController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Loads Talents, Jobs and Job Applications
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoadAppData(IFormFile formFile)
        {
            var response = await _service.Dataload.RunDataloadAsync(formFile);
            if (!response.Success)
            {
                return ProcessError(response); 
            }
            return Ok(response.GetResult<string>());
        }
    }
}
