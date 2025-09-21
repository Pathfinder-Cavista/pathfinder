namespace PathFinder.Application.Commands.Accounts
{
    public class TalentProfileUpdateCommand
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        public string Location { get; set; } = string.Empty;
        public string ProfileSummary { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public ICollection<string> Skills { get; set; } = [];
    }
}
