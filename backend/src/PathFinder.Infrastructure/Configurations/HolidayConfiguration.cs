using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PathFinder.Application.Helpers;
using PathFinder.Domain.Entities;

namespace PathFinder.Infrastructure.Configurations
{
    internal class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> builder)
        {
            builder.HasData(
                new Holiday
                {
                    Id = Guid.Parse("9e41a7c3-3da9-4efd-98e7-74b3120babad"),
                    Name = "New Year's Day",
                    Country = "Global",
                    Date = new DateTime(2025, 1, 1).ToUtcDate(),
                    IsRecurring = true
                },
                new Holiday
                {
                    Id = Guid.Parse("9e41a7c3-3da9-4efd-98e7-74b3120badab"),
                    Name = "Worker's Day",
                    Country = "Nigeria",
                    Date = new DateTime(2025, 5, 1).ToUtcDate(),
                    IsRecurring = true
                },
                new Holiday
                {
                    Id = Guid.Parse("e941a7c3-3da9-4efd-98e7-74b3120babad"),
                    Name = "Democracy Day",
                    Country = "Nigeria",
                    Date = new DateTime(2025, 6, 12).ToUtcDate(),
                    IsRecurring = true
                },
                new Holiday
                {
                    Id = Guid.Parse("9e4a17c3-3da9-4efd-98e7-74b3120babad"),
                    Name = "Independence Day",
                    Country = "Nigeria",
                    Date = new DateTime(2025, 10, 1).ToUtcDate(),
                    IsRecurring = true
                },
                new Holiday
                {
                    Id = Guid.Parse("9e41a73c-3ad9-4efd-98e7-74b3120babad"),
                    Name = "Christmas Day",
                    Country = "Global",
                    Date = new DateTime(2025, 12, 25).ToUtcDate(),
                    IsRecurring = true
                },
                new Holiday
                {
                    Id = Guid.Parse("9e41a73c-3ad9-ef4d-98e7-74b3120babad"),
                    Name = "Boxing Day",
                    Country = "Nigeria",
                    Date = new DateTime(2025, 12, 26).ToUtcDate(),
                    IsRecurring = true
                }
            );
        }
    }
}
