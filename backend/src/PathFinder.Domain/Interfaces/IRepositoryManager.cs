namespace PathFinder.Domain.Interfaces
{
    public interface IRepositoryManager
    {
        ISampleRepository Sample { get; }
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
