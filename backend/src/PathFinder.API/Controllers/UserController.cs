using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Mappers;
using PathFinder.API.Requests.Accounts;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _service;

        public UserController(IServiceManager serviceManager)
        {
            _service = serviceManager;
        }

        /// <summary>
        /// Get logged in user details
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var userDetails = await _service.Account.GetLoggedInUserdetails();
            return Ok(userDetails);
        }

        /// <summary>
        /// Update recruiter profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("update-recruiter")]
        [Authorize(Roles = "Manager, Admin")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRecruiterProfile(RecruiterProfileUpdateRequest request)
        {
            return Ok(await _service.Account
                .UpdateRecruiterProfileAsync(AccountRequestMappers.ToProfileUpdateCommand(request)));
        }

        /// <summary>
        /// Update talent profile details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("update-talent")]
        [Authorize(Roles = "Talent")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTalentProfile(TalentProfileUpdateRequest request)
        {
            return Ok(await _service.Account
                .UpdateTalentProfileAsync(AccountRequestMappers.ToProfileUpdateCommand(request)));
        }
    }
}
