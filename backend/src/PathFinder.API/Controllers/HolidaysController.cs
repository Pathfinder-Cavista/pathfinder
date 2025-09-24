using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PathFinder.API.Controllers.Extensions;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Interfaces;

namespace PathFinder.API.Controllers
{
    [Route("api/holidays")]
    [ApiController]
    public class HolidaysController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public HolidaysController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets a list of holdidays
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(typeof(List<HolidayDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.Holiday.GetHolidayList();
            if (result.Success)
            {
                return ProcessError(result);
            }

            return Ok(result.GetResult<List<HolidayDto>>());
        }

        /// <summary>
        /// Adds a holiday
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(EntityIdDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> Post(CreateHolidayCommand command)
        {
            var result = await _service.Holiday.AddHolidayAsync(command);
            if (result.Success)
            {
                return ProcessError(result);
            }

            return Ok(result.GetResult<EntityIdDto>());
        }

        /// <summary>
        /// Updates a holiday record
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(EntityIdDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> Put(UpdateHolidayCommand command)
        {
            var result = await _service.Holiday.UpdateHolidayAsync(command);
            if (result.Success)
            {
                return ProcessError(result);
            }

            return Ok(result.GetResult<EntityIdDto>());
        }

        /// <summary>
        /// Deletes a holiday record
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.Holiday.DeleteHolidayAsync(id);
            if (result.Success)
            {
                return ProcessError(result);
            }

            return Ok(result.GetResult<string>());
        }
    }
}
