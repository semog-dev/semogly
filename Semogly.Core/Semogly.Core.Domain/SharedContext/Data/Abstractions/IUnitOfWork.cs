namespace Semogly.Core.Domain.SharedContext.Data.Abstractions;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}