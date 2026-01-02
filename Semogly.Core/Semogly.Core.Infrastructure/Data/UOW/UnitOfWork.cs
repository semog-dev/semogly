using Semogly.Core.Domain.SharedContext.Data.Abstractions;

namespace Semogly.Core.Infrastructure.Data.UOW;

public class UnitOfWork(SemoglyDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}