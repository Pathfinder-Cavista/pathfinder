using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathFinder.Application.Helpers;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;

namespace PathFinder.Infrastructure.Repositories
{
    public class DataloadRepository : IDataloadRepository
    {
        private readonly AppDbContext _context;

        public DataloadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddDataAsync(List<AppUser> users,
                                       List<Job> jobs,
                                       List<JobApplication> applications)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Jobs.AddRange(jobs);
                _context.Users.AddRange(users);
                _context.Applications.AddRange(applications);
                await _context.SaveChangesAsync();

                var talentRole = _context.Roles.FirstOrDefault(r => r.Name == Roles.Talent.GetDescription());
                if(talentRole != null)
                {
                    var userRoles = new List<IdentityUserRole<string>>();
                    foreach(var user in users)
                    {
                        userRoles.Add(new IdentityUserRole<string>
                        {
                            UserId = user.Id,
                            RoleId = talentRole.Id
                        });
                    }

                    await _context.UserRoles.AddRangeAsync(userRoles);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}