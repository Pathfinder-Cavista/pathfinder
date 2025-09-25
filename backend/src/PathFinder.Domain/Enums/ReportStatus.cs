using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum ReportStatus
    {
        [Description("Running")]
        Running,
        [Description("Completed")]
        Completed,
        [Description("Failed")]
        Failed
    }
}
