using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathFinder.API.Extensions;
using PathFinder.API.Filters;
using PathFinder.Application.Helpers;
using PathFinder.Application.Mappers;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;
using PathFinder.Infrastructure.Persistence;

namespace PathFinder.API.Extensions
{
    public static class WebAppExtensions
    {
        private const string organizationName = "Cavista Technologies";
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
            await SeedOrganization(scope, logger);
            await SeedAdminUsers(scope, logger);
        }

        private static async Task SeedAdminUsers(IServiceScope scope, ILogger<Program> logger)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if(userManager == null || config == null || context == null)
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
            var organization = await context.Set<Organization>()
                .FirstOrDefaultAsync(o => o.Name == organizationName);

            if (adminUsers.Count != 0 && organization != null)
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
                        Title = "Platform Admin.",
                        OrganizationId = organization.Id
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        logger.LogError($"Unable to add {user.FirstName}: {result.Errors.FirstOrDefault()?.Description}");
                        continue;
                    }

                    var roleResult = await userManager.AddToRoleAsync(user, Domain.Enums.Roles.Admin.GetDescription());
                    if (!roleResult.Succeeded)
                    {
                        await userManager.DeleteAsync(user);
                        logger.LogError($"Unable to add {user.FirstName} to the Admin role: {roleResult.Errors.FirstOrDefault()?.Description}");
                        continue;
                    }

                    logger.LogInformation($"{user.FirstName} successfully registered");
                }
            }
            else
            {
                logger.LogInformation("Seeding skipped... Users already exist in the DB");
            }
        }

        private static async Task SeedOrganization(IServiceScope scope, ILogger<Program> logger)
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (context == null || userManager == null)
            {
                logger.LogWarning("Required services are not instantialted");
                return;
            }

            var existingOrg = await context.Set<Organization>()
                .FirstOrDefaultAsync(o => o.Name == organizationName);

            if(existingOrg == null)
            {
                var organization = new Organization
                {
                    Name = organizationName,
                    Vision = "To be the most innovative and respected technology solutions provider in the global market",
                    Mission = "To empower our clients with the world's best technology solutions",
                    About = @"Cavista Technologies is a leader in providing software engineering and development services to orgainzations around the world. 
With engineers and industry experts accross the globe, Cavista provides around-the-clock support that streamlines processes and minimizes project delivery time."
                };

                logger.LogInformation($"Adding {organizationName} to Organizations table");
                await context.Set<Organization>().AddAsync(organization);
                await context.SaveChangesAsync();
                logger.LogInformation($"Added {organizationName} to Organizations table");

                return;
            }
            else
            {
                logger.LogInformation($"{organizationName} is already in the DB.");
                return;
            }
        }
    }
}
