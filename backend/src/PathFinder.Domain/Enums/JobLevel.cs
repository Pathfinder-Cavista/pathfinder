using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum JobLevel
    {
        [Description("Entry")]
        Entry,
        [Description("Junior")]
        Junior,
        [Description("Mid-level")]
        Mid,
        [Description("Senior")]
        Senior,
        [Description("Lead")]
        Lead,
        [Description("Manager")]
        Manager,
        [Description("Director")]
        Director
    }
}
