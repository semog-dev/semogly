using Semogly.Core.Domain.SharedContext.Aggregates.Abstractions;

namespace Semogly.Core.Domain.SharedContext.Repositories.Abstractions;

public interface IRepository<TEntity, TKey> where TEntity : IAggregateRoot
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default);
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    void UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
}