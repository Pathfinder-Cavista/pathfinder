using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathFinder.Application.Helpers;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Enums;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

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

        public async Task AddReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Report>> GetReportsAsync(Expression<Func<Report, bool>> expression)
        {
            return await _context.Reports.AsQueryable()
                .Where(expression)
                .OrderByDescending(r => r.CreatedAt)
                .Take(20)
                .ToListAsync();
        }

        public async Task<Report?> GetReportAsync(Expression<Func<Report, bool>> expression)
        {
            return await _context.Set<Report>()
                .FirstOrDefaultAsync(expression);
        }

        public async Task EditReportAsync(Report report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
        }
    }
}