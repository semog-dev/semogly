using System;
using Microsoft.EntityFrameworkCore;
using Semogly.Core.Domain.SharedContext.Aggregates.Abstractions;
using Semogly.Core.Domain.SharedContext.Entities;
using Semogly.Core.Domain.SharedContext.Repositories.Abstractions;

namespace Semogly.Core.Infrastructure.Data.Repositories;

public abstract class Repository<TEntity, TKey>(SemoglyDbContext dbContext) : IRepository<TEntity, TKey>
    where TEntity: Entity<TKey>, IAggregateRoot
    where TKey : notnull
{
    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>().FindAsync([id], cancellationToken);
    }

    public async Task<TEntity?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.PublicId == publicId, cancellationToken);
    }

    public void UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<TEntity>().Update(entity);
    }
}