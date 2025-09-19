using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum EmploymentType
    {
        [Description("Full-Time")]
        Fulltime,
        [Description("Part-Time")]
        PartTime,
        [Description("Contract")]
        Contract,
        [Description("Internship")]
        Internship,
        [Description("Temporary")]
        Temporary
    }
}
