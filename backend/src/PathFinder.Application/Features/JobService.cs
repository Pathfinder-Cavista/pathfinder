using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PathFinder.Application.Commands.Jobs;
using PathFinder.Application.Helpers;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Responses;
using PathFinder.Application.Validations.Jobs;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;

namespace PathFinder.Application.Features
{
    public class JobService : IJobService
    {
        private readonly IRepositoryManager _repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public JobService(IRepositoryManager repository,
                          UserManager<AppUser> userManager,
                          IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<ApiBaseResponse> PostJobAsync(PostJobCommand command)
        {
            var validator = new PostJobCommandValidator().Validate(command);
            if (!validator.IsValid)
            {
                return new BadRequestResponse(validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid inputs");
            }

            var loggedInUserId = AccountHelpers
                .GetLoggedInUserId(_contextAccessor.HttpContext.User);
            if (string.IsNullOrWhiteSpace(loggedInUserId))
            {
                return new ForbiddenResponse("You're not authorized to perform this operation.");
            }

            return new OkResponse<string>("");
        }
    }
}