namespace PathFinder.API.Requests.Accounts
{
    public class AuthRequest
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
