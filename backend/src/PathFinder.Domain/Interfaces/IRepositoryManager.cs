namespace PathFinder.Domain.Interfaces
{
    public interface IRepositoryManager
    {
        ITokenRepository Token { get; }
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
