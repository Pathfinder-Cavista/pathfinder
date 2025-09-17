using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Mappers;
using PathFinder.API.Requests;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/sample")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IServiceManager _servvice;

        public SampleController(IServiceManager servvice)
        {
            _servvice = servvice;
        }

        /// <summary>
        /// Creates a sample
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Sample object</returns>
        /// <response code="200">Returns the sample object</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="500">If an unexpected error occurred</response>
        [HttpPost]
        [ProducesResponseType(typeof(SampleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreateSampleRequest request)
        {
            var command = SampleRequestMapper.ToSampleCommand(request);
            return Ok(await _servvice.Sample.AddSample(command));
        }
    }
}