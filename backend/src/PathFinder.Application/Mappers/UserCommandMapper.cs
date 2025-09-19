using PathFinder.Application.Commands.Accounts;
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
    }
}