using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Controllers.Extensions;
using PathFinder.API.Mappers;
using PathFinder.API.Requests.Jobs;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public JobsController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Posts a new job
        /// </summary>
        /// <param name="request">The application payload
        /// <br/><br/>
        /// <b>Accepted values for <c>Employment Type</c>: </b>
        /// <list type="bullet">
        /// <item><description>0 = Full Time, </description></item>
        /// <item><description>1 = Part Time, </description></item>
        /// <item><description>2 = Contract, </description></item>
        /// <item><description>3 = Internship, </description></item>
        /// <item><description>4 = Temporary</description></item>
        /// </list>
        /// <br/><br/>
        /// <b>Accepted values for <c>Job Type</c>: </b>
        /// <list type="bullet">
        /// <item><description>0 = Entry, </description></item>
        /// <item><description>1 = Junior, </description></item>
        /// <item><description>2 = Mid, </description></item>
        /// <item><description>3 = Senior, </description></item>
        /// <item><description>4 = Lead, </description></item>
        /// <item><description>5 = Manager, </description></item>
        /// <item><description>6 = Director</description></item>
        /// </list>
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(PostJobRequest request)
        {
            var command = JobRequestsMapper.ToPostJobCommand(request);
            var baseResult = await _service.Job.PostJobAsync(command);
            if (!baseResult.Success)
            {
                return ProcessError(baseResult);
            }

            return Ok(baseResult.GetResult<EntityIdDto>());
        }

        /// <summary>
        /// Updates a job record
        /// </summary>
        /// <param name="id">the id of the record to be updated</param>
        /// <param name="request">The application payload
        /// <br/><br/>
        /// <b>Accepted values for <c>Employment Type</c>: </b>
        /// <list type="bullet">
        /// <item><description>0 = Full Time, </description></item>
        /// <item><description>1 = Part Time, </description></item>
        /// <item><description>2 = Contract, </description></item>
        /// <item><description>3 = Internship, </description></item>
        /// <item><description>4 = Temporary</description></item>
        /// </list>
        /// <br/><br/>
        /// <b>Accepted values for <c>Job Type</c>: </b>
        /// <list type="bullet">
        /// <item><description>0 = Entry, </description></item>
        /// <item><description>1 = Junior, </description></item>
        /// <item><description>2 = Mid, </description></item>
        /// <item><description>3 = Senior, </description></item>
        /// <item><description>4 = Lead, </description></item>
        /// <item><description>5 = Manager, </description></item>
        /// <item><description>6 = Director</description></item>
        /// </list>
        /// </param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] PatchJobRequest request)
        {
            var command = JobRequestsMapper.ToPatchJobCommand(id, request);
            var baseResult = await _service.Job.PatchJobAsync(command);
            if (!baseResult.Success)
            {
                return ProcessError(baseResult);
            }

            return Ok(baseResult.GetResult<EntityIdDto>());
        }

        /// <summary>
        /// Gets jobs details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(JobDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var baseResult = await _service.Job.GetByIdAsync(id);
            if (!baseResult.Success)
            {
                return ProcessError(baseResult);
            }

            return Ok(baseResult.GetResult<JobDetailDto>());
        }

        /// <summary>
        /// Deletes a job record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(EntityIdDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Deprecate([FromRoute] Guid id)
        {
            var baseResult = await _service.Job.DeprecateJobAsync(id);
            if (!baseResult.Success)
            {
                return ProcessError(baseResult);
            }

            return Ok(baseResult.GetResult<EntityIdDto>());
        }

        [HttpGet]
        public IActionResult GetPagedJobs()
        {
            var pagedJobsResult = _service.Job
                .GetPaginatedJobs(new Application.Commands.Jobs.JobQuery
                {

                });

            if (!pagedJobsResult.Success)
            {
                return ProcessError(pagedJobsResult);
            }

            return Ok(pagedJobsResult.GetResult<Paginator<LeanJobDto>>());
        }
    }
}
