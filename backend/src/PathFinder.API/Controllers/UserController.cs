using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Controllers.Extensions;
using PathFinder.API.Mappers;
using PathFinder.API.Requests.Accounts;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ApiControllerBase
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
        [ProducesResponseType(typeof(UserBaseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var isInTalentRole = HttpContext.User.IsInRole("Talent");
            var userDetailsResult = isInTalentRole ? 
                await _service.Account.GetLoggedInTalentDetails() : 
                await _service.Account.GetLoggedInRecruiterDetails();
            
            if (!userDetailsResult.Success)
            {
                return ProcessError(userDetailsResult);
            }

            if (isInTalentRole)
            {
                return Ok(userDetailsResult.GetResult<TalentInfoDto>());
            }
            else
            {
                return Ok(userDetailsResult.GetResult<RecruiterInfoDto>());
            }
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
            var baseResponse = await _service.Account
                .UpdateRecruiterProfileAsync(AccountRequestMappers.ToProfileUpdateCommand(request));
            if (!baseResponse.Success)
            {
                return ProcessError(baseResponse);
            }

            return Ok(baseResponse.GetResult<string>());
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
            var baseResult = await _service.Account
                .UpdateTalentProfileAsync(AccountRequestMappers.ToProfileUpdateCommand(request));
            if (!baseResult.Success)
            {
                return ProcessError(baseResult);
            }

            return Ok(baseResult.GetResult<string>());
        }

       /// <summary>
       /// Uploads user's profile picture
       /// </summary>
       /// <param name="file"></param>
       /// <returns></returns>
        [HttpPatch("upload-profile-image")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadPicture(IFormFile file)
        {
            var baseResult = await _service.Account
                .UploadProfileImage(file);
            if (!baseResult.Success)
            {
                return ProcessError(baseResult);
            }

            return Ok(baseResult.GetResult<string>());
        }

        /// <summary>
        /// Uploads talent's CV
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPatch("upload-cv")]
        [Authorize(Roles = "Talent")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadCV(IFormFile file)
        {
            var baseResult = await _service.Account
                .UploadResumeAsync(file);
            if (!baseResult.Success)
            {
                return ProcessError(baseResult);
            }

            return Ok(baseResult.GetResult<string>());
        }
    }
}
