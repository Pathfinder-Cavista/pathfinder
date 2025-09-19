using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum JobApplicationStatus
    {
        [Description("Applied")]
        Applied,
        [Description("In Review")]
        InReview,
        [Description("Interviewed")]
        Interviewed,
        [Description("Offer Extended")]
        OfferExtended,
        [Description("Hired")]
        Hired,
        [Description("Rejected")]
        Rejected,
        [Description("Withdrawn")]
        Withdrawn
    }
}
