using Microsoft.AspNetCore.Http;
using PathFinder.Application.Commands.Accounts;
using PathFinder.Application.Responses;

namespace PathFinder.Application.Interfaces
{
    public interface IAccountService
    {
        Task<ApiBaseResponse> GetLoggedInRecruiterDetails();
        Task<ApiBaseResponse> GetLoggedInTalentDetails();
        Task<ApiBaseResponse> LoginAsync(LoginCommand command);
        Task<ApiBaseResponse> RefreshTokenAsync(RefreshTokenCommand command);
        Task<ApiBaseResponse> RegisterAsync(RegisterCommand command);
        Task<ApiBaseResponse> UpdateRecruiterProfileAsync(RecruiterProfileUpdateCommand command);
        Task<ApiBaseResponse> UpdateTalentProfileAsync(TalentProfileUpdateCommand command);
        Task<ApiBaseResponse> UploadProfileImage(IFormFile formFile);
        Task<ApiBaseResponse> UploadResumeAsync(IFormFile formFile);
    }
}
