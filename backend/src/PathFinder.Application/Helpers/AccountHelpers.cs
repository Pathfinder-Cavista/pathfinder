using System.Security.Claims;

namespace PathFinder.Application.Helpers
{
    public static class AccountHelpers
    {
        public static string? GetLoggedInUserId(ClaimsPrincipal? claim)
        {
            return claim?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
