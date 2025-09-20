using System.Security.Claims;
using System.Text.RegularExpressions;

namespace PathFinder.Application.Helpers
{
    public static class AccountHelpers
    {
        public static string? GetLoggedInUserId(ClaimsPrincipal? claim)
        {
            return claim?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static bool IsAValidPhoneNumber(string phoneNumber)
        {
            var pattern = @"^\+(\d{1,4})[-\s]?(\d{6,14})$";
            Match match = Regex.Match(phoneNumber, pattern);
            return match.Success;
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list is not null && list.Any();
        }
    }
}
