using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Controllers.Extensions;
using PathFinder.API.Mappers;
using PathFinder.API.Requests;
using PathFinder.API.Requests.Jobs;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;
using PathFinder.Domain.Enums;

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

        /// <summary>
        /// Gets paged lists of jobs.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paginator<LeanJobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult GetPagedJobs([FromQuery] JobQueryRequest query)
        {
            var pagedJobsResult = _service.Job
                .GetPaginatedJobs(JobRequestsMapper.MapJobQuery(query));

            if (!pagedJobsResult.Success)
            {
                return ProcessError(pagedJobsResult);
            }

            return Ok(pagedJobsResult.GetResult<Paginator<LeanJobDto>>());
        }

        /// <summary>
        /// Endpoint for talents to apply for jobs
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/apply")]
        [Authorize(Roles = "Talent")]
        [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Apply([FromRoute] Guid id)
        {
            var response = await _service.Job.ApplyAsync(id);
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<ApplicationDto>());
        }

        /// <summary>
        /// Endpoint for getting paged list of applications for a job. Only accessible to Talent Managers and Admins
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        [HttpGet("{id}/applications")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(Paginator<ApplicationDataDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Applications([FromRoute] Guid id,
                                                        [FromQuery] PageQueryRequest pageQuery)
        {
            var response = await _service.Job
                .GetJobApplicationsAsync(JobRequestsMapper.MapToApplicationQueries(id, pageQuery));
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<Paginator<AdminApplicationDataDto>>());
        }

        /// <summary>
        /// Endpoint for getting paged list of talent's job applications
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        [HttpGet("applications")]
        [Authorize(Roles = "Talent")]
        [ProducesResponseType(typeof(Paginator<ApplicationDataDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Applications([FromQuery] PageQueryRequest pageQuery)
        {
            var response = await _service.Job
                .GetTalentJobApplicationsAsync(JobRequestsMapper.MapPageQueries(pageQuery));
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<Paginator<ApplicationDataDto>>());
        }

        /// <summary>
        /// Endpoint for getting a job application by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        [HttpGet("{id}/applications/{applicationId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApplicationDataDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Application([FromRoute] Guid id, 
                                                        [FromRoute] Guid applicationId)
        {
            var response = await _service.Job
                .GetApplicationAsync(applicationId, id);
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<ApplicationDataDto>());
        }

       /// <summary>
       /// Gets summary data for jobs
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        [HttpGet("{id}/summary")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(JobDetailsForDashboardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> JobSummaryForDashboard([FromRoute] Guid id)
        {
            var response = await _service.Job
                .GetJobForDashboard(id);
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<JobDetailsForDashboardDto>());
        }

        /// <summary>
        /// Change application status
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="status"> Valid statuses: 
        /// <list type="bullet">
        /// <item><description><c>In Review - 1</c></description></item>
        /// <item><description><c>Interviewing - 2</c></description></item>
        /// <item><description><c>Interviewed - 3</c></description></item>
        /// <item><description><c>Offer Extended - 4</c></description></item>
        /// <item><description><c>Hired - 5</c></description></item>
        /// <item><description><c>Rejected - 6</c></description></item>
        /// </list>
        /// </param>
        /// <returns></returns>
        [HttpPatch("applications/{applicationId}/{status}")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(ApplicationDataDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid applicationId,
                                                        [FromRoute] JobApplicationStatus status)
        {
            var response = await _service.Job
                .ChangeApplicationStatus(new PathFinder.Application.Commands.Jobs.ApplicationStatusCommand
                {
                    ApplicationId = applicationId,
                    NewStatus = status
                });
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<string>());
        }
    }
}
