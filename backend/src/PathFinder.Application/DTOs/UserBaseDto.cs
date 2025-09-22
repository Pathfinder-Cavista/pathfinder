using PathFinder.Domain.Entities;

namespace PathFinder.Application.DTOs
{
    public class UserBaseDto
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public DateTime? LastLogin { get; set; }  
    }

    public class TalentInfoDto : UserBaseDto
    {
        public string? Location { get; set; }
        public string? CareerSummary { get; set; }
        public string? ResumeUrl { get; set; }
        public List<string> Skills { get; set; } = [];

        public static TalentInfoDto ToTalentInfoDto(AppUser appUser, TalentProfile? profile)
        {
            var info = new TalentInfoDto
            {
                Id = appUser.Id,
                FullName = string.Concat(appUser.FirstName, " ", appUser.LastName),
                Email = appUser.Email!,
                Phone = appUser.PhoneNumber!,
                LastLogin = appUser.LastLogin,
                PhotoUrl = appUser.ProfilePhoto
            };

            if(profile != null)
            {
                info.Location = profile.Location;
                info.ResumeUrl = profile.ResumeUrl;
                info.CareerSummary = profile.Summary;
            }

            return info;
        }
    }

    public class RecruiterInfoDto : UserBaseDto
    {
        public string? Title { get; set; }
        public string? OrganizationName { get; set; }

        public static RecruiterInfoDto ToRecruiterInfoDto(AppUser appUser, RecruiterProfile? profile)
        {
            var info = new RecruiterInfoDto
            {
                Id = appUser.Id,
                FullName = string.Concat(appUser.FirstName, " ", appUser.LastName),
                Email = appUser.Email!,
                Phone = appUser.PhoneNumber!,
                LastLogin = appUser.LastLogin,
                PhotoUrl= appUser.ProfilePhoto,     
            };

            if (profile != null)
            {
                info.Title = profile.Title;
                if(profile.Organization != null)
                {
                    info.OrganizationName = profile.Organization.Name;
                }
            }

            return info;
        }
    }
}
