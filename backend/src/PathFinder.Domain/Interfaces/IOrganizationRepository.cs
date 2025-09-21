using PathFinder.Domain.Entities;
using System.Linq.Expressions;

namespace PathFinder.Domain.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<Organization?> GetAsync(Expression<Func<Organization, bool>> expression);
    }
}
