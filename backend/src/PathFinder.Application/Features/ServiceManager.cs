using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Settings;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;

namespace PathFinder.Application.Features
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<IJobService> _jobService;
        private readonly Lazy<IDataloadService> _dataloadService;
        private readonly Lazy<IAnalyticsService> _analyticsService;
        private readonly Lazy<IHolidayService> _holidayService;

        public ServiceManager(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              IHttpContextAccessor contextAccessor,
                              IRepositoryManager repository,
                              IOptions<JwtSettings> options,
                              IUploadService uploadService,
                              IEligibilityService eligibility)
        {
            _accountService = new Lazy<IAccountService>(() 
                => new AccountService(userManager, signInManager, contextAccessor, repository, options, uploadService));

            _jobService = new Lazy<IJobService>(()
                => new JobService(repository, contextAccessor, userManager));

            _dataloadService = new Lazy<IDataloadService>(()
                => new DataloadService(userManager, contextAccessor, eligibility, repository));

            _analyticsService = new Lazy<IAnalyticsService>(()
                => new AnalyticsService(repository, contextAccessor, userManager, uploadService));

            _holidayService = new Lazy<IHolidayService>(()
                => new HolidayService(repository));
        }

        public IAccountService Account => _accountService.Value;
        public IJobService Job => _jobService.Value;
        public IDataloadService Dataload => _dataloadService.Value;
        public IAnalyticsService Analytics => _analyticsService.Value;
        public IHolidayService Holiday => _holidayService.Value;
    }
}
