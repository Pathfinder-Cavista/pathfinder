using Microsoft.AspNetCore.Mvc;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Responses;

namespace PathFinder.API.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ProcessError(ApiBaseResponse response)
        {
            return response switch
            {
                NotFoundResponse => NotFound(new ErrorResponse
                {
                    Message = ((NotFoundResponse)response).Message,
                    Status = StatusCodes.Status400BadRequest
                }),
                BadRequestResponse => BadRequest(new ErrorResponse
                {
                    Message = ((BadRequestResponse)response).Message,
                    Status = StatusCodes.Status400BadRequest
                }),
                ForbiddenResponse => StatusCode(StatusCodes.Status403Forbidden,
                new ErrorResponse
                {
                    Message = ((ForbiddenResponse)response).Message,
                    Status = StatusCodes.Status403Forbidden
                }),
                _ => throw new NotImplementedException()
            };
        }
    }
}