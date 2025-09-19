using Hangfire;
using Hangfire.Console;
using Hangfire.MySql;
using Hangfire.RecurringJobExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PathFinder.Application.Features;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Settings;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using PathFinder.Infrastructure.Repositories;
using System.Reflection;
using System.Text;

namespace PathFinder.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services,
                                            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentNullException(nameof(configuration));

            services.ConfigureHangfire(configuration)
                .RegisterDbContext(connectionString)
                .RegisterRepository()
                .RegisterServices()
                .ConfigureSwaggerDocs()
                .ConfigureJwt(configuration);
        }

        private static IServiceCollection ConfigureHangfire(this IServiceCollection services,
                                                            IConfiguration configuration)
        {
            services.AddHangfire((provider, config) =>
            {
                config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(new MySqlStorage(configuration.GetConnectionString("DefaultConnection"), new MySqlStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromSeconds(10),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 25000,
                    TransactionTimeout = TimeSpan.FromMinutes(1),
                    TablesPrefix = "Hangfire",
                }))

                .WithJobExpirationTimeout(TimeSpan.FromHours(6))
                .UseConsole()
                .UseRecurringJob(typeof(IRecurringJobService))
                .UseFilter(new AutomaticRetryAttribute()
                {
                    Attempts = 5,
                    DelayInSecondsByAttemptFunc = _ => 30
                });
            })
            .AddHangfireServer(options => // Configure Hangfire server
            {
                options.ServerName = "Hangfire Background Jobs Server";
                options.Queues = new[] { "recurring", "default" };
                options.SchedulePollingInterval = TimeSpan.FromMinutes(1);
                options.WorkerCount = 5;
            });

            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services,
                                                           string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    m => m.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            return services;
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            return services;
        }

        private static IServiceCollection ConfigureSwaggerDocs(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlFilePath);

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Auth. Header using Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        private static IServiceCollection ConfigureJwt(this IServiceCollection services,
                                                       IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSection);
            var settings = jwtSection.Get<JwtSettings>() ?? 
                throw new ArgumentNullException("JwtSettings");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = settings.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.PrivateKey))
                };
            });

            services.AddAuthorization();

            return services;
        }
    }
}