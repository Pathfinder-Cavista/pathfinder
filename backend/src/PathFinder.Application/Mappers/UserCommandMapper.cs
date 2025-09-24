using DocumentFormat.OpenXml.Drawing;
using PathFinder.Application.Commands.Accounts;
using PathFinder.Application.DTOs;
using PathFinder.Domain.Entities;

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

        public static AppUser ToAppUser(DataloadUserProfile dataloadUser, string cvUrl)
        {
            var user = new AppUser
            {
                FirstName = dataloadUser.FirstName,
                LastName = dataloadUser.LastName,
                Email = dataloadUser.Email,
                PhoneNumber = dataloadUser.PhoneNumber,
                EmailConfirmed = true,
                UserName = dataloadUser.Email
            };

            user.Talent = new TalentProfile
            {
                Summary = dataloadUser.Summary,
                Location = dataloadUser.City,
                YearsOfExperience = dataloadUser.YearsOfExperience,
                ResumeUrl = cvUrl,
                UserId = user.Id,
            };

            return user;
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