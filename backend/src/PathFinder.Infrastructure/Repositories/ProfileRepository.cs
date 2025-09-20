using Microsoft.EntityFrameworkCore;
using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _context;

        public ProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TalentProfile?> GetTalentProfileAsync(Expression<Func<TalentProfile, bool>> expression)
        {
            var profile = await _context.Talents
                .FirstOrDefaultAsync(expression);

            return profile;
        }

        public async Task<RecruiterProfile?> GetRecruiterProfileAsync(Expression<Func<RecruiterProfile, bool>> expression)
        {
            var profile = await _context.Recruiters
                .FirstOrDefaultAsync(expression);

            return profile;
        }
    }
}
