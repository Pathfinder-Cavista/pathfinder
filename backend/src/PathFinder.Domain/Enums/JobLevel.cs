using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum JobLevel
    {
        /// <summary>
        /// Entry
        /// </summary>
        [Description("Entry")]
        Entry,
        /// <summary>
        /// Junior
        /// </summary>
        [Description("Junior")]
        Junior,
        /// <summary>
        /// Mid
        /// </summary>
        [Description("Mid-level")]
        Mid,
        /// <summary>
        /// Senior
        /// </summary>
        [Description("Senior")]
        Senior,
        /// <summary>
        /// Lead
        /// </summary>
        [Description("Lead")]
        Lead,
        /// <summary>
        /// Manager
        /// </summary>
        [Description("Manager")]
        Manager,
        /// <summary>
        /// Director
        /// </summary>
        [Description("Director")]
        Director
    }
}
