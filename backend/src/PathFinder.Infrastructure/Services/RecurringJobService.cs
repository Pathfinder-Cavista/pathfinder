using Hangfire.Console;
using Hangfire.Server;
using PathFinder.Application.Interfaces;

namespace PathFinder.Infrastructure.Services
{
    public class RecurringJobService : IRecurringJobService
    {
        public async Task PollSmartRecruiterApiForUpdates(PerformContext context)
        {
            context.WriteLine("Smart Recruiters Polling started...");
            await Task.CompletedTask;
        }
    }
}