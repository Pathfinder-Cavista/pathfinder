namespace PathFinder.Application.Settings
{
    public class JwtSettings
    {
        public string PrivateKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int Expires { get; set; }
    }
}
