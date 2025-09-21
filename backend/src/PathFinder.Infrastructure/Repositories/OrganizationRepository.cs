using PathFinder.Domain.Entities;
using PathFinder.Domain.Interfaces;
using PathFinder.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace PathFinder.Infrastructure.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<Organization?> GetAsync(Expression<Func<Organization, bool>> expression)
        {
            return await FindOneAsync(expression);
        }
    }
}
