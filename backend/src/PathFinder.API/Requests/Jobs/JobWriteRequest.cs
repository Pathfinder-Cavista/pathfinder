using PathFinder.Domain.Enums;

namespace PathFinder.API.Requests.Jobs
{
    public abstract class JobWriteRequest
    {
        /// <summary>
        /// Job title
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Job description
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Closing date
        /// </summary>
        public DateTime? DeadLine { get; set; }
        /// <summary>
        /// List of requirements for the job
        /// </summary>
        public ICollection<string> Requirements { get; set; } = [];
        /// <summary>
        /// Flag to set status as publish or draft. Defaults to true
        /// </summary>
        public bool PublishNow { get; set; } = true;
        public EmploymentType EmploymentType { get; set; }
        public JobLevel Level { get; set; }
        public string? Location { get; set; }
        public List<string> Skills { get; set; } = [];
    }
}
