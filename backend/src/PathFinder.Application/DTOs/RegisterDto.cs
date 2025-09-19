using PathFinder.Domain.Entities;

namespace PathFinder.Application.DTOs
{
    public class RegisterDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public static RegisterDto FromEntity(AppUser user)
        {
            return new RegisterDto
            {
                Id = user.Id,
                Email = user.Email!
            };
        }
    }
}
