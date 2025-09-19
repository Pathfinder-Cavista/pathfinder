using PathFinder.Application.Commands.Accounts;
using PathFinder.Application.DTOs;

namespace PathFinder.Application.Interfaces
{
    public interface IAccountService
    {
        Task<UserBaseDto?> GetLoggedInUserdetails();
        Task<TokenDto> LoginAsync(LoginCommand command);
        Task<TokenDto> RefreshTokenAsync(RefreshTokenCommand command);
        Task<RegisterDto> RegisterAsync(RegisterCommand command);
    }
}
