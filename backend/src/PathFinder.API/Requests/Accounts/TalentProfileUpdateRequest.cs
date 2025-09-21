namespace PathFinder.API.Requests.Accounts
{
    public class TalentProfileUpdateRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? OtherName { get; set; }
        public string City { get; set; } = string.Empty;
        public string CareerSummary { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public ICollection<string> ProfileSkills { get; set; } = [];
    }
}
