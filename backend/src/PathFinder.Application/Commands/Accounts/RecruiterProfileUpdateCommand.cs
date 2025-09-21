namespace PathFinder.Application.Commands.Accounts
{
    public class RecruiterProfileUpdateCommand
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
