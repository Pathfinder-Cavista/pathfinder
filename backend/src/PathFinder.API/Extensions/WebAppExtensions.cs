using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathFinder.API.Extensions;
using PathFinder.API.Filters;
using PathFinder.Application.Helpers;
using PathFinder.Application.Mappers;
using PathFinder.Domain.Entities;
using PathFinder.Infrastructure.Persistence;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PathFinder.API.Extensions
{
    public static class WebAppExtensions
    {
        internal static WebApplication RunMigrations(this WebApplication app, bool alwayRun = false)
        {
            if (app.Environment.IsDevelopment() || alwayRun)
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            return app;
        }

        internal static WebApplication UseHangfireDashboard(this WebApplication app, IConfiguration configuration)
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

        internal static async Task SeedInitialDataAsync(this WebApplication app, ILogger<Program> logger)
        {
            using var scope = app.Services.CreateScope();
            await SeedAdminUsers(scope, logger);
        }

        private static async Task SeedAdminUsers(IServiceScope scope, ILogger<Program> logger)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            if(userManager == null || config == null)
            {
                logger.LogWarning("Required services are not instantialted");
                return;
            }

            var password = config["HangfireSettings:Password"];
            var adminUsers = new List<(string Email, string FirstName, string LastName)>
            {
                ("cike-nwako@axxess.com", "Chukwudi", "Ike-nwako"), 
                ("sgar@axxess.com", "Shalom", "Gar"), 
                ("fonafowokan@axxess.com", "Folusho", "Onafowokan"), 
                ("oehilen@axxess.com", "Osehiase", "Ehilen"), 
                ("tojo@axxess.com", "Toba", "Ojo")
            };

            if (string.IsNullOrWhiteSpace(password))
            {
                logger.LogWarning("Password is null or empty");
                return;
            }

            var existingUsers = await userManager.Users
                .Where(u => adminUsers.Select(e => e.Email).Contains(u.Email!))
                .ToListAsync();

            var missingUserEmails = adminUsers.Select(e => e.Email)
                .Except(existingUsers.Select(u => u.Email!), StringComparer.OrdinalIgnoreCase)
                .ToList();

            adminUsers.RemoveAll(e => !missingUserEmails.Contains(e.Email));
            if (adminUsers.Count != 0)
            {
                foreach (var userTuple in adminUsers)
                {
                    var user = UserCommandMapper.ToAppUser(new Application.Commands.Accounts.RegisterCommand
                    {
                        FirstName = userTuple.FirstName,
                        LastName = userTuple.LastName,
                        Email = userTuple.Email,
                        PhoneNumber = "+2348031234567"
                    });

                    user.Recruiter = new RecruiterProfile
                    {
                        UserId = user.Id,
                        Title = "Platform Admin."
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        logger.LogError($"Unable to add {user.FirstName}: {result.Errors.FirstOrDefault()?.Description}");
                        return;
                    }

                    var roleResult = await userManager.AddToRoleAsync(user, Domain.Enums.Roles.Admin.GetDescription());
                    if (!roleResult.Succeeded)
                    {
                        await userManager.DeleteAsync(user);
                        logger.LogError($"Unable to add {user.FirstName} to the Admin role: {roleResult.Errors.FirstOrDefault()?.Description}");
                        return;
                    }

                    logger.LogInformation($"{user.FirstName} successfully registered");
                }
            }
            else
            {
                logger.LogInformation("Seeding skipped... Users already exist in the DB");
            }
        }
    }
}
