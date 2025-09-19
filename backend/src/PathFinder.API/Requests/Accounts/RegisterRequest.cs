using PathFinder.Domain.Enums;

namespace PathFinder.API.Requests.Accounts
{
    public class RegisterRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Roles UserRole { get; set; } = Roles.Talent;
    }
}
