using PathFinder.Domain.Entities;

namespace PathFinder.Domain.Interfaces
{
    public interface ISampleRepository
    {
        Task AddAsync(Sample sample);
    }
}
