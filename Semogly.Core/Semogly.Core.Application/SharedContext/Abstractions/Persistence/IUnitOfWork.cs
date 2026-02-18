namespace Semogly.Core.Application.SharedContext.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}