using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum EmploymentType
    {
        /// <summary>
        /// Full-time
        /// </summary>
        [Description("Full-Time")]
        Fulltime,
        /// <summary>
        /// Part-time
        /// </summary>
        [Description("Part-Time")]
        PartTime,
        /// <summary>
        /// Contract
        /// </summary>
        [Description("Contract")]
        Contract,
        /// <summary>
        /// Internship
        /// </summary>
        [Description("Internship")]
        Internship,
        /// <summary>
        /// Temporary
        /// </summary>
        [Description("Temporary")]
        Temporary
    }
}
