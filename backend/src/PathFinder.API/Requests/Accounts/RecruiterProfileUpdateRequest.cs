namespace PathFinder.API.Requests.Accounts
{
    public class RecruiterProfileUpdateRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        public string ProfileTitle { get; set; } = string.Empty;
    }
}
