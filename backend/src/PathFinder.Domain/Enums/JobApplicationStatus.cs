using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum JobApplicationStatus
    {
        /// <summary>
        /// Applied
        /// </summary>
        [Description("Applied")]
        Applied,
        /// <summary>
        /// In Review
        /// </summary>
        [Description("In Review")]
        InReview,
        /// <summary>
        /// Interviewing
        /// </summary>
        [Description("Interviewing")]
        Interviewing,
        /// <summary>
        /// Interviewed
        /// </summary>
        [Description("Interviewed")]
        Interviewed,
        /// <summary>
        /// Offer Extended
        /// </summary>
        [Description("Offer Extended")]
        OfferExtended,
        /// <summary>
        /// Hired
        /// </summary>
        [Description("Hired")]
        Hired,
        /// <summary>1
        /// Rejected
        /// </summary>
        [Description("Rejected")]
        Rejected,
        /// <summary>
        /// Withdrawn
        /// </summary>
        [Description("Withdrawn")]
        Withdrawn
    }
}
