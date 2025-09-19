using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PathFinder.Application.Helpers;
using PathFinder.Domain.Enums;

namespace PathFinder.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = new Guid("665376b2-6d04-42d6-95a8-4a14e1819649").ToString(),
                    Name = Roles.Talent.ToString(),
                    NormalizedName = Roles.Talent.GetDescription().ToUpper(),
                    ConcurrencyStamp = DateTime.UtcNow.ToString(),
                },
                 new IdentityRole
                 {
                     Id = new Guid("e6a2221f-9e15-4474-9264-73a76447849e").ToString(),
                     Name = Roles.Manager.ToString(),
                     NormalizedName = Roles.Manager.GetDescription().ToUpper(),
                     ConcurrencyStamp = DateTime.UtcNow.ToString(),
                 },
                  new IdentityRole
                  {
                      Id = new Guid("b35f5379-539e-413c-8ebc-e407fdf705c2").ToString(),
                      Name = Roles.Admin.ToString(),
                      NormalizedName = Roles.Admin.GetDescription().ToUpper(),
                      ConcurrencyStamp = DateTime.UtcNow.ToString(),
                  }
            );
        }
    }
}
