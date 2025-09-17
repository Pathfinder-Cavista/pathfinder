using Hangfire;
using Microsoft.EntityFrameworkCore;
using PathFinder.API.Extensions;
using PathFinder.API.Filters;
using PathFinder.Infrastructure.Persistence;

namespace PathFinder.API.Extensions
{
    public static class WebAppExtensions
    {
        public static WebApplication RunMigrations(this WebApplication app, bool alwayRun = false)
        {
            if (app.Environment.IsDevelopment() || alwayRun)
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            return app;
        }

        public static WebApplication UseHangfireDashboard(this WebApplication app, IConfiguration configuration)
        {
            app.UseHangfireDashboard("/admin/jobs", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter(configuration) },
                DashboardTitle = "Jobs Dashboard",
                DisplayStorageConnectionString = false,
                DisplayNameFunc = (_, job) => job.Method.Name,
                DarkModeEnabled = true,
            });

            return app;
        }
    }
}
