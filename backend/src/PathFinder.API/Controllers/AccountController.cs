using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Mappers;
using PathFinder.API.Requests.Accounts;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
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
            return Ok(await _service.Account.LoginAsync(command));
        }

        /// <summary>
        ///  Registers a user
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Returns the sample object</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="404">If user not found</response>
        /// <response code="500">If an unexpected error occurred</response>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var command = AccountRequestMappers.ToRegisterCommand(request);
            return Ok(await _service.Account.RegisterAsync(command));
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
            return Ok(await _service.Account.RefreshTokenAsync(command));
        }
    }
}