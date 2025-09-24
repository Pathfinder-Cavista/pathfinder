namespace PathFinder.Application.DTOs
{
    public class DataloadUserProfile
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public List<string> ProfileSkills { get; set; } = [];
    }
}
