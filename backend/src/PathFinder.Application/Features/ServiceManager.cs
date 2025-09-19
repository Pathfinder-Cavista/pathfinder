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

        public ServiceManager(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              IHttpContextAccessor contextAccessor,
                              IRepositoryManager repository,
                              IOptions<JwtSettings> options)
        {
            _accountService = new Lazy<IAccountService>(() 
                => new AccountService(userManager, signInManager, contextAccessor, repository, options));
        }

        public IAccountService Account => _accountService.Value;
    }
}
