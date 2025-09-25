using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PathFinder.Domain.Entities;
using PathFinder.Infrastructure.Configurations;

namespace PathFinder.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<RefreshToken> Tokens { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> Applications { get; set; }
        public DbSet<RecruiterProfile> Recruiters { get; set; }
        public DbSet<TalentProfile> Talents { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<TalentSkill> TalentSkills { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<JobRequirement> JobRequirements { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Report> Reports { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TalentSkill>()
                .HasOne(ts => ts.TalentProfile)
                .WithMany(s => s.Skills)
                .HasForeignKey(t => t.TalentProfileId);

            builder.Entity<TalentSkill>()
                .HasOne(ts => ts.Skill)
                .WithMany()
                .HasForeignKey(s => s.SkillId);

            builder.Entity<JobSkill>()
                .HasKey(js => new { js.JobId, js.SkillId });

            builder.Entity<JobSkill>()
                .HasOne(js => js.Job)
                .WithMany(j => j.RequiredSkills)
                .HasForeignKey(j => j.JobId);

            builder.Entity<JobSkill>()
                .HasOne(js => js.Skill)
                .WithMany()
                .HasForeignKey(j => j.SkillId);

            builder.Entity<TalentSkill>()
                .HasIndex(ts => new { ts.TalentProfileId, ts.SkillId })
                .IsUnique();

            builder.Entity<JobSkill>()
                .HasIndex(js => new {  js.JobId, js.SkillId })
                .IsUnique();

            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new HolidayConfiguration());
        }
    }
}