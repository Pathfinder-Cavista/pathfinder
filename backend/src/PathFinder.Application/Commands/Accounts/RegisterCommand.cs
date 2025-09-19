using PathFinder.Domain.Enums;

namespace PathFinder.Application.Commands.Accounts
{
    public class RegisterCommand
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Roles Role { get; set; } = Roles.Talent;
    }
}
