using PathFinder.API.Requests.Accounts;
using PathFinder.Application.Commands.Accounts;

namespace PathFinder.API.Mappers
{
    public static class AccountRequestMappers
    {
        public static LoginCommand ToLoginCommand(AuthRequest request)
        {
            return new LoginCommand
            {
               Email = request.EmailAddress,
               Password = request.Password,
            };
        }

        public static RegisterCommand ToRegisterCommand(RegisterRequest request)
        {
            return new RegisterCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                OtherName = request.OtherName,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                Email = request.EmailAddress,
                Role = request.UserRole
            };
        }

        public static RefreshTokenCommand ToTokenCommand(RefreshTokenRequest request)
        {
            return new RefreshTokenCommand
            {
                RefreshToken = request.Token
            };
        }

        public static RecruiterProfileUpdateCommand ToProfileUpdateCommand(RecruiterProfileUpdateRequest request)
        {
            return new RecruiterProfileUpdateCommand
            {
                Title = request.ProfileTitle
            };
        }

        public static TalentProfileUpdateCommand ToProfileUpdateCommand(TalentProfileUpdateRequest request)
        {
            return new TalentProfileUpdateCommand
            {
                Address = request.HomeAddress,
                Location = request.City,
                ProfileSummary = request.CareerSummary,
                Skills = request.ProfileSkills
            };
        }
    }
}
