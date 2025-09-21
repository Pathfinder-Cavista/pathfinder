using PathFinder.Application.Commands.Accounts;
using PathFinder.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PathFinder.Application.Mappers
{
    public class UserCommandMapper
    {
        public static AppUser ToAppUser(RegisterCommand registerCommand)
        {
            return new AppUser
            {
                FirstName = registerCommand.FirstName,
                LastName = registerCommand.LastName,
                Email = registerCommand.Email,
                MiddleName = registerCommand.OtherName,
                PhoneNumber = registerCommand.PhoneNumber,
                EmailConfirmed = true,
                UserName = registerCommand.Email
            };
        }

        public static void UpdateTalentProfile(AppUser user, TalentProfileUpdateCommand command, TalentProfile? profile)
        {
            if (profile == null)
            {
                profile = new TalentProfile
                {
                    Location = command.Location,
                    Summary = command.ProfileSummary,
                    UserId = user.Id,
                    YearsOfExperience = command.YearsOfExperience
                };
            }

            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.MiddleName = command.OtherName;
            profile.Location = command.Location;
            profile.Summary = command.ProfileSummary;
            profile.YearsOfExperience = command.YearsOfExperience;
        }

        public static void UpdateRecruiterProfile(AppUser user, RecruiterProfileUpdateCommand command)
        {
            user.Recruiter ??= new RecruiterProfile
            {
                UserId = user.Id,
                Title = command.Title,
            };

            user.Recruiter.Title = command.Title;
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.MiddleName = command.OtherName;
        }
    }
}