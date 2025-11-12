using Semogly.Core.Domain.SharedContext.Data.Abstractions;

namespace Semogly.Core.Data.AccountContext;

public class UnitOfWork(AccountDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}