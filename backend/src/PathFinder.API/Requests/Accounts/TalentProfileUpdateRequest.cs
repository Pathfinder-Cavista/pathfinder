namespace PathFinder.API.Requests.Accounts
{
    public class TalentProfileUpdateRequest
    {
        public string City { get; set; } = string.Empty;
        public string HomeAddress { get; set; } = string.Empty;
        public string CareerSummary { get; set; } = string.Empty;
        public ICollection<string> ProfileSkills { get; set; } = [];
    }
}
