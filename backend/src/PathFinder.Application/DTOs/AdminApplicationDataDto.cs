using System.Text.Json.Serialization;

namespace PathFinder.Application.DTOs
{
    public class AdminApplicationDataDto
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string ApplicantFullName { get; set; } = string.Empty;
        public string? ResumeUrl { get; set; } = string.Empty;
        public Guid TalentId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public string JobStatus { get; set; } = string.Empty;
        public string ApplicationStatus { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public bool IsEligible { get; set; }
        public string EligibilityThreshold => $"{Threshold * 100}%";
        [JsonIgnore]
        public double Threshold { get; set; }

    }
}