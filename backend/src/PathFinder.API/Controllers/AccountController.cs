using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Controllers.Extensions;
using PathFinder.API.Mappers;
using PathFinder.API.Requests.Accounts;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public AccountController(IServiceManager servvice)
        {
            _service = servvice;
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Returns the sample object</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="404">If user not found</response>
        /// <response code="500">If an unexpected error occurred</response>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync(AuthRequest request)
        {
            var command = AccountRequestMappers.ToLoginCommand(request);
            var baseResult = await _service.Account.LoginAsync(command);
            if (!baseResult.Success)
            {
                return ProcessError(baseResult);
            }

            return Ok(baseResult.GetResult<TokenDto>());
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="request">Registration payload
        /// <br/><br/>
        /// <b>Accepted values for <c>Role</c>: </b>
        /// <list type="bullet">
        /// <item><description>0 = Talent, </description></item>
        /// <item><description>1 = Manager, </description></item>
        /// <item><description>2 = Admin</description></item>
        /// </list>
        /// </param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var command = AccountRequestMappers.ToRegisterCommand(request);
            var baseResult = await _service.Account.RegisterAsync(command);
            if(!baseResult.Success)
            {
                return ProcessError(baseResult);
            }

            return Ok(baseResult.GetResult<RegisterDto>());
        }

        /// <summary>
        /// Refreshes access token
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Returns the sample object</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="404">If user not found</response>
        /// <response code="500">If an unexpected error occurred</response>
        /// <returns></returns>
        [HttpPut("refresh")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request)
        {
            var command = AccountRequestMappers.ToTokenCommand(request);
            var baseResult = await _service.Account.RefreshTokenAsync(command);
            if (!baseResult.Success)
            {
                return ProcessError(baseResult);
            }

            return Ok(baseResult.GetResult<TokenDto>());
        }
    }
}