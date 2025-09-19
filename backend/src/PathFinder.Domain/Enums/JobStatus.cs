using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum JobStatus
    {
        [Description("Draft")]
        Draft,
        [Description("Published")]
        Published,
        [Description("Closed")]
        Closed,
        [Description("Archived")]
        Archived
    }
}
