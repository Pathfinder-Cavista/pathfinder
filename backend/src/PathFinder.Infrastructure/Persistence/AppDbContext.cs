using Microsoft.EntityFrameworkCore;
using PathFinder.Domain.Entities;

namespace PathFinder.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Sample> Samples { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 
    }
}