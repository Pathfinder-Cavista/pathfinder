namespace PathFinder.Application.Commands.Accounts
{
    public class TalentProfileUpdateCommand
    {
        public string Location { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ProfileSummary { get; set; } = string.Empty;
        public ICollection<string> Skills { get; set; } = [];
    }
}
