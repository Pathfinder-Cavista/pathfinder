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
using PathFinder.Application.Responses;
using PathFinder.Application.Settings;
using PathFinder.Application.Validations.Accounts;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;
using PathFinder.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PathFinder.Application.Features
{
    public sealed class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        //private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClaimsPrincipal? _claim;
        private readonly IRepositoryManager _repository;
        private readonly IUploadService _uploadService;
        private readonly JwtSettings _settings;

        public AccountService(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              IHttpContextAccessor contextAccessor,
                              IRepositoryManager repository,
                              IOptions<JwtSettings> options,
                              IUploadService uploadService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _claim = contextAccessor.HttpContext?.User;
            _repository = repository;
            _uploadService = uploadService;
            _settings = options.Value;
        }

        public async Task<ApiBaseResponse> LoginAsync(LoginCommand command)
        {
            var validator = new LoginCommandValidator().Validate(command);
            if (!validator.IsValid)
            {
                return new BadRequestResponse(validator.Errors?.FirstOrDefault()?.ErrorMessage ?? "Invalid input");
            }

            var user = await _userManager.FindByEmailAsync(command.Email) ?? 
                throw new NotFoundException("User not found");

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: true);
            if (!passwordCheck.Succeeded)
            {
                return new BadRequestResponse("Wrong email or password");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if(roles is null || !roles.Any())
            {
                return new ForbiddenResponse("You can not login at this time.");
            }

            var accessToken = CreateAccessToken(user, [.. roles]);
            var refreshToken = await CreateAndSaveRefreshToken(user.Id);

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return new OkResponse<TokenDto>(new TokenDto(accessToken, refreshToken));
        }

        public async Task<ApiBaseResponse> RegisterAsync(RegisterCommand command)
        {
            var (canRegister, orgId) = await ValidateRole(command.Role);
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

            await AddTalentOrRecruiterProfile(user, new List<string> { command.Role.GetDescription() }, orgId);
            return new OkResponse<RegisterDto>(RegisterDto.FromEntity(user));
        }

        public async Task<ApiBaseResponse> RefreshTokenAsync(RefreshTokenCommand command)
        {
            var tokenFromDb = await ValidateRefreshToken(command.RefreshToken);
            if(tokenFromDb == null)
            {
                return new ForbiddenResponse("Invalid or expired token");
            }

            var user = await _userManager.FindByIdAsync(tokenFromDb.UserId);
            if (user == null)
            {
                return new NotFoundResponse("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = CreateAccessToken(user, roles.ToArray());
            return new OkResponse<TokenDto>(new TokenDto(newAccessToken, command.RefreshToken));
        }

        public async Task<ApiBaseResponse> GetLoggedInRecruiterDetails()
        {
            var loggedInUserId = AccountHelpers.GetLoggedInUserId(_claim);
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                return new ForbiddenResponse("User not authenticated");
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == loggedInUserId);

            if (user == null)
            {
                return new NotFoundResponse("User not found");
            }

            var recruiterProfile = await _repository.RecruiterProfile
                .GetAsync(u => u.UserId == loggedInUserId, false, true);

            var roles = await _userManager.GetRolesAsync(user);
            if(recruiterProfile == null)
            {
                await AddTalentOrRecruiterProfile(user, roles.ToList());
            }

            return new OkResponse<RecruiterInfoDto>(RecruiterInfoDto.ToRecruiterInfoDto(user, recruiterProfile));
        }

        public async Task<ApiBaseResponse> GetLoggedInTalentDetails()
        {
            var loggedInUserId = AccountHelpers.GetLoggedInUserId(_claim);
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                return new ForbiddenResponse("User not authenticated");
            }

            var user = await _userManager.Users
                .Include(u => u.Talent)
                .FirstOrDefaultAsync(u => u.Id == loggedInUserId);

            if (user == null)
            {
                return new NotFoundResponse("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            await AddTalentOrRecruiterProfile(user, roles.ToList());
            var talentData = await _repository.TalentProfile
                    .GetAsync(t => t.UserId == loggedInUserId);

            var info = TalentInfoDto.ToTalentInfoDto(user, talentData);
            if (talentData != null)
            {
                var skills = await _repository.TalentSkill
                    .GetAsync(s => s.TalentProfileId == talentData.Id);

                skills.ForEach(ts =>
                {
                    if (ts.Skill != null)
                    {
                        info.Skills.Add(ts.Skill.Name);
                    }
                });
            }

            return new OkResponse<TalentInfoDto>(info);
        }

        public async Task<ApiBaseResponse> UpdateRecruiterProfileAsync(RecruiterProfileUpdateCommand command)
        {
            var loggedInUserId = AccountHelpers.GetLoggedInUserId(_claim);
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                return new ForbiddenResponse("User not authenticated");
            }

            var user = await _userManager.Users
                .Include(u => u.Recruiter)
                .FirstOrDefaultAsync(u => u.Id == loggedInUserId);
            if (user == null)
            {
                return new NotFoundResponse("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains(Roles.Admin.GetDescription()) && !roles.Contains(Roles.Manager.GetDescription()))
            {
                return new ForbiddenResponse("You have no permission to perform this operation");
            }

            UserCommandMapper.UpdateRecruiterProfile(user, command);
            await _userManager.UpdateAsync(user);

            return new OkResponse<string>("Profile successfully updated");
        }

        public async Task<ApiBaseResponse> UpdateTalentProfileAsync(TalentProfileUpdateCommand command)
        {
            var validator = new TalentProfileUpdateCommandValidator().Validate(command);
            if (!validator.IsValid)
            {
                return new BadRequestResponse(validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid inputs");
            }

            var loggedInUserId = AccountHelpers.GetLoggedInUserId(_claim);
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                return new ForbiddenResponse("User not authenticated");
            }

            var user = await _userManager.FindByIdAsync(loggedInUserId);
            if (user == null)
            {
                return new NotFoundResponse("User not found");
            }

            var isATalent = await _userManager.IsInRoleAsync(user, Roles.Talent.GetDescription());
            if (!isATalent)
            {
                return new ForbiddenResponse("You have no permission to perform this operation");
            }

            var talentProfile = await _repository.TalentProfile
                .GetAsync(p => p.UserId == loggedInUserId, true);

            UserCommandMapper.UpdateTalentProfile(user, command, talentProfile);
            await _userManager.UpdateAsync(user);
            await _repository.SaveAsync();

            await HandleSkillsUpdate(talentProfile!, command.Skills);
            return new OkResponse<string>("Profile successfully updated");
        }

        public async Task<ApiBaseResponse> UploadProfileImage(IFormFile formFile)
        {
            var validation = formFile.IsAValidFile(UploadMediaType.Image);
            if (!validation.Valid)
            {
                return new BadRequestResponse(validation.Message);
            }

            var userId = AccountHelpers.GetLoggedInUserId(_claim);
            if(string.IsNullOrWhiteSpace(userId))
            {
                return new ForbiddenResponse("Access denied.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new NotFoundResponse("User not found");
            }

            await using var stream = formFile.OpenReadStream();
            var result = await _uploadService.UploadImageAsync(userId.Replace("-", ""), formFile.FileName, stream);
            if(result == null)
            {
                return new BadRequestResponse("Image file upload. Please try again shortly");
            }

            user.ProfilePhoto = result.Url;
            user.ProfilePhotoPublicId = result.PublicId;
            await _userManager.UpdateAsync(user);

            return new OkResponse<string>("Profile image successfully uploaded");
        }

        public async Task<ApiBaseResponse> UploadResumeAsync(IFormFile formFile)
        {
            var validation = formFile.IsAValidFile(UploadMediaType.Document);
            if (!validation.Valid)
            {
                return new BadRequestResponse(validation.Message);
            }

            var userId = AccountHelpers.GetLoggedInUserId(_claim);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new ForbiddenResponse("Access denied.");
            }

            var user = await _userManager.Users
                .Include(u => u.Talent)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || user.Talent == null)
            {
                return new NotFoundResponse("User profile not found");
            }

            await using var stream = formFile.OpenReadStream();
            var result = await _uploadService.UploadRawAsync(userId.Replace("-", ""), formFile.FileName, stream);
            if (result == null)
            {
                return new BadRequestResponse("Image file upload. Please try again shortly");
            }

            user.Talent.ResumeUrl = result.Url;
            user.Talent.ResumePublicId = result.PublicId;
            await _userManager.UpdateAsync(user);

            return new OkResponse<string>("CV successfully uploaded");
        }

        #region Private Methods
        public async Task HandleSkillsUpdate(TalentProfile talent, ICollection<string> talentSkills)
        {
            var normalizedNames = talentSkills.Select(n => n.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingSkills = await _repository.Skill
                .AsQueryable(s => normalizedNames.Contains(s.Name))
                .ToListAsync();

            var missingNames = normalizedNames
                .Except(existingSkills.Select(s => s.Name), StringComparer.OrdinalIgnoreCase)
                .ToList();

            var newSkills = missingNames.Select(name => new Skill
            {
                Name = name.CapitalizeFirstLetterOnly(),
            }).ToList();

            await _repository.Skill.AddRangeAsync(newSkills);

            var allSkills = existingSkills.Concat(newSkills).ToList();
            var userExistingSkills = await _repository.TalentSkill
                .GetAsync(s => s.TalentProfileId == talent.Id);

            var skillsToAdd = new List<TalentSkill>();

            foreach (var skill in allSkills)
            {
                skillsToAdd.Add(new TalentSkill
                {
                    TalentProfileId = talent.Id,
                    SkillId = skill.Id,
                });
            }

            await _repository.TalentSkill.RemoveManyAsync(userExistingSkills, false);
            await _repository.TalentSkill.AddRangeAsync(skillsToAdd, false);
            await _repository.SaveAsync();
        }

        private async Task AddTalentOrRecruiterProfile(AppUser user, List<string> roles, Guid? orgId = null)
        {
           if(user.Talent is null && user.Recruiter is null)
           {
                if (roles.Contains(Roles.Talent.GetDescription()))
                {
                    var profile = new TalentProfile
                    {
                        UserId = user.Id
                    };

                    await _repository.TalentProfile.AddAsync(profile);
                }
                else
                {
                    var profile = new RecruiterProfile
                    {
                        UserId = user.Id,
                        OrganizationId = orgId.HasValue ? orgId.Value : Guid.Empty
                    };

                    await _repository.RecruiterProfile.AddAsync(profile);
                }
           }
        }
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

        private async Task<(bool Valid, Guid? OrgId)> ValidateRole(Roles role)
        {
            if(role == Roles.Talent)
            {
                return (true, null);
            }

            var loggedInUserId = AccountHelpers.GetLoggedInUserId(_claim);
            if(string.IsNullOrEmpty(loggedInUserId))
            {
                return (false, null);
            }

            var user = await _userManager.FindByIdAsync(loggedInUserId);
            if (user == null)
            {
                return (false, null);
            }

            var profile = await _repository.RecruiterProfile
                .GetAsync(p => p.UserId == user.Id, false, true);
            if(profile == null || profile.Organization == null)
            {
                return (false, null);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return (roles.Contains(Roles.Admin.ToString()), profile.OrganizationId);
        }

        #endregion
    }
}
