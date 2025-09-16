using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace PathFinder.Application.Interfaces
{
    public interface IRecurringJobService
    {
        [RecurringJob("0 */6 * * *", TimeZone = "UTC")]
        Task PollSmartRecruiterApiForUpdates(PerformContext context);
    }
}