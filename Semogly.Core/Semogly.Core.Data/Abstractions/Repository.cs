using Microsoft.EntityFrameworkCore;
using Semogly.Core.Domain.Abstractions;
using Semogly.Core.Domain.Abstractions.Interfaces;
using Semogly.Core.Data.Extensions;

namespace Semogly.Core.Data.Abstractions;

public class Repository<TEntity, TKey>(DbContext dbContext) : IRepository<TEntity> where TEntity : Entity, IAggregateRoot
{
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>()
            .AddAsync(entity, cancellationToken);
    }

    public void Delete(TEntity entity)
    {
        dbContext.Set<TEntity>()
            .Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginationResult<TEntity>> GetAllPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>()
            .AsNoTracking()
            .ToPaginationResultAsync(page, pageSize, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>()
            .Update(entity);
    }
}