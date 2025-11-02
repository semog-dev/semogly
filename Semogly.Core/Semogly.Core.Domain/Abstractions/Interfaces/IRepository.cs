namespace Semogly.Core.Domain.Abstractions.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity, IAggregateRoot
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaginationResult<TEntity>> GetAllPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}