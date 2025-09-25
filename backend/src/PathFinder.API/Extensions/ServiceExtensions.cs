using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
                .AddScoped<IUploadService, UploadService>()
                .AddScoped<IEligibilityService, EligibilityService>()
                .AddScoped<IRepositoryManager, RepositoryManager>()
                .AddScoped<IServiceManager, ServiceManager>()
                .ConfigureSwaggerDocs()
                .ConfigureJwt(configuration)
                .ConfigureCors()
                .ConfigureCloudinary(configuration)
                .ConfigureAnalyticsSettings(configuration);        
        }

        private static IServiceCollection ConfigureAnalyticsSettings(this IServiceCollection services,
                                                                        IConfiguration configuration)
        {
            var analyticsSettings = configuration.GetSection("Analytics");
            return services
                .Configure<AnalyticsSettings>(analyticsSettings);
        }

        private static IServiceCollection ConfigureCloudinary(this IServiceCollection services,
                                                                IConfiguration configuration)
        {
            var config = configuration.GetSection("Cloudinary");

            return services
                .Configure<CloudinarySettings>(config)
                .AddSingleton<IUploadService>(sp =>
                {
                    var setting = sp.GetService<IOptions<CloudinarySettings>>() ?? 
                        throw new ArgumentNullException(nameof(CloudinarySettings));

                    return new UploadService(setting);
                });
        }

        private static IServiceCollection ConfigureHangfire(this IServiceCollection services,
                                                            IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(opt =>
                    {
                        opt.UseNpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
                    })
                    .UseConsole()
                    .UseFilter(new AutomaticRetryAttribute()
                    {
                        Attempts = 5,
                        DelayInSecondsByAttemptFunc = _ => 30
                    });
            }).AddHangfireServer(opt =>
            {
                opt.ServerName = "Hangfire Background Jobs Server";
                opt.Queues = new[] { "recurring", "default" };
                opt.SchedulePollingInterval = TimeSpan.FromMinutes(1);
                opt.WorkerCount = 5;
            });

            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services,
                                                           string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString,
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

                opt.AddSecurityDefinition("X-Insights", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "X-Insights",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Custom Header"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "X-Insights"
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

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.PrivateKey))
                };
            });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", b =>
                {
                    b.WithOrigins("https://localhost:4200", "http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
                });
            });

            return services;
        }
    }
}