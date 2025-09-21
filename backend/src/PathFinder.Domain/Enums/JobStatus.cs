using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum JobStatus
    {
        /// <summary>
        /// Draft
        /// </summary>
        [Description("Draft")]
        Draft,
        /// <summary>
        /// Published
        /// </summary>
        [Description("Published")]
        Published,
        /// <summary>
        /// Closed
        /// </summary>
        [Description("Closed")]
        Closed,
        /// <summary>
        /// Archived
        /// </summary>
        [Description("Archived")]
        Archived
    }
}
