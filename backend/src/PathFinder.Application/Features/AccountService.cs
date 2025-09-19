using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PathFinder.Application.Commands.Accounts;
using PathFinder.Application.DTOs;
using PathFinder.Application.Exceptions;
using PathFinder.Application.Helpers;
using PathFinder.Application.Interfaces;
using PathFinder.Application.Mappers;
using PathFinder.Application.Settings;
using PathFinder.Application.Validations.Accounts;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;
using PathFinder.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.Application.Features
{
    public sealed class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRepositoryManager _repository;
        private readonly JwtSettings _settings;

        public AccountService(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              IHttpContextAccessor contextAccessor,
                              IRepositoryManager repository,
                              IOptions<JwtSettings> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _repository = repository;
            _settings = options.Value;
        }

        public async Task<TokenDto> LoginAsync(LoginCommand command)
        {
            var validator = new LoginCommandValidator().Validate(command);
            if (!validator.IsValid)
            {
                throw new BadRequestException(validator.Errors?.FirstOrDefault()?.ErrorMessage ?? "Invalid input");
            }

            var user = await _userManager.FindByEmailAsync(command.Email) ?? 
                throw new NotFoundException("User not found");

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: true);
            if (!passwordCheck.Succeeded)
            {
                throw new BadRequestException("Wrong email or password");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if(roles is null || !roles.Any())
            {
                throw new ForbiddenException("You can not login at this time.");
            }

            var accessToken = CreateAccessToken(user, [.. roles]);
            var refreshToken = await CreateAndSaveRefreshToken(user.Id);

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return new TokenDto(accessToken, refreshToken);
        }

        public async Task<RegisterDto> RegisterAsync(RegisterCommand command)
        {
            var canRegister = await ValidateRole(command.Role);
            if (!canRegister)
            {
                throw new ForbiddenException("You are not allowed to perform this action");
            }

            var modelValidation = new RegisterCommandValidator().Validate(command);
            if (!modelValidation.IsValid)
            {
                throw new BadRequestException(modelValidation.Errors?.FirstOrDefault()?.ErrorMessage ?? "Invalid inputs");
            }

            var user = UserCommandMapper.ToAppUser(command);
            var createResult = await _userManager.CreateAsync(user, command.Password);
            if (!createResult.Succeeded)
            {
                throw new BadRequestException(createResult.Errors?.FirstOrDefault()?.Description ?? "Registration failed");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, command.Role.ToString());
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new BadRequestException(roleResult.Errors.FirstOrDefault()?.Description ?? "Registration failed");
            }

            return RegisterDto.FromEntity(user);
        }

        public async Task<TokenDto> RefreshTokenAsync(RefreshTokenCommand command)
        {
            var tokenFromDb = await ValidateRefreshToken(command.RefreshToken) ?? 
                throw new ForbiddenException("Invalid or expired token");

            var user = await _userManager.FindByIdAsync(tokenFromDb.UserId) ?? 
                throw new NotFoundException("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = CreateAccessToken(user, roles.ToArray());
            return new TokenDto(newAccessToken, command.RefreshToken);
        }

        public async Task<UserBaseDto?> GetLoggedInUserdetails()
        {
            var loggedInUserId = AccountHelpers.GetLoggedInUserId(_contextAccessor.HttpContext?.User);
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                throw new ForbiddenException("User not authenticated");
            }

            var user = await _userManager.Users//.Include(u => u.Talent).Include(u => u.Recruiter)
                .FirstOrDefaultAsync(u => u.Id == loggedInUserId) ??
                    throw new NotFoundException("User not found");
            
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains(Roles.Talent.GetDescription()) ?
                TalentInfoDto.ToTalentInfoDto(user, user.Talent) :
                RecruiterInfoDto.ToRecruiterInfoDto(user, user.Recruiter);
        }

        #region Private Methods
        private async Task<RefreshToken?> ValidateRefreshToken(string token)
        {
            var hash = ComputeHash(token);
            var tokenEntity = await _repository.Token.GetAsync(x => x.Hash == hash && !x.IsDeprecated, true);
            if(tokenEntity == null)
            {
                return null;
            }

            if(tokenEntity.ExpiresAt < DateTimeOffset.UtcNow)
            {
                tokenEntity.IsDeprecated = true;
                await _repository.SaveAsync();
                return null;
            }

            return tokenEntity;
        }
        private string CreateAccessToken(AppUser user, string[] roles)
        {
            var claims = GetClaims(user, roles);
            var credentials = GetSigningCredentials();
            var securityToken = GetJwtSecurityToken(claims, credentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private JwtSecurityToken GetJwtSecurityToken(List<Claim> claims, SigningCredentials credentials)
        {
            return new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(_settings.Expires),
                signingCredentials: credentials
            );
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_settings.PrivateKey);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims(AppUser user, string[] roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Iss, _settings.Issuer),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        private async Task<string> CreateAndSaveRefreshToken(string userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var hash = ComputeHash(token);

            await _repository.Token.AddAsync(new RefreshToken
            {
                Hash = hash,
                UserId = userId,
                ExpiresAt = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(_settings.Expires))
            });

            return token;
        }
        private string ComputeHash(string token)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        private async Task<bool> ValidateRole(Roles role)
        {
            if(role == Roles.Talent)
            {
                return true;
            }

            var loggedInUserId = AccountHelpers.GetLoggedInUserId(_contextAccessor.HttpContext?.User);
            if(string.IsNullOrEmpty(loggedInUserId))
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(loggedInUserId);
            if (user == null)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles.Contains(Roles.Manager.ToString()) || roles.Contains(Roles.Admin.ToString());
        }

        #endregion
    }
}
