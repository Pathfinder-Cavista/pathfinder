using PathFinder.Domain.Enums;

namespace PathFinder.API.Requests.Jobs
{
    public class JobQueryRequest : PageQueryRequest
    {
        /// <summary>
        /// Sort by Creation Date (Default: DESC)<br />
        /// <list type="bullet">
        /// <item><description><c>Ascending Order - ASC</c></description></item>
        /// <item><description><c>Descending Order - DESC</c></description></item>
        /// </list>
        /// </summary>
        public string SortOrder { get; set; } = "DESC";
        public string? Search { get; set; }

        /// <summary>
        ///
        /// <list type="bullet">
        /// <item><description><c>Draft - 0</c></description></item>
        /// <item><description><c>Open - 1</c></description></item>
        /// <item><description><c>Closed - 2</c></description></item>
        /// <item><description><c>Archived - 3</c></description></item>
        /// </list>
        /// </summary>
        public JobStatus? Status { get; set; }
        /// <summary>
        ///
        /// <list type="bullet">
        /// <item><description><c>Full-Time - 0</c></description></item>
        /// <item><description><c>Part-Time - 1</c></description></item>
        /// <item><description><c>Contract - 2</c></description></item>
        /// <item><description><c>Internship - 3</c></description></item>
        /// <item><description><c>Temporary - 4</c></description></item>
        /// </list>
        /// </summary>
        public EmploymentType? Type { get; set; }
        /// <summary>
        ///
        /// <list type="bullet">
        /// <item><description><c>Entry Level - 0</c></description></item>
        /// <item><description><c>Junior - 1</c></description></item>
        /// <item><description><c>Mid-Level - 2</c></description></item>
        /// <item><description><c>Senior - 3</c></description></item>
        /// <item><description><c>Lead - 4</c></description></item>
        /// <item><description><c>Manager - 5</c></description></item>
        /// <item><description><c>Director - 6</c></description></item>
        /// </list>
        /// </summary>
        public JobLevel? Level { get; set; }
    }
}
