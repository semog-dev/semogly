using Microsoft.EntityFrameworkCore;
using Semogly.Core.Application.SharedContext.Abstractions.Persistence;
using Semogly.Core.Domain.AccountContext.Entities;

namespace Semogly.Core.Infrastructure.Persistence.Repositories;

public class AccountRepository(SemoglyDbContext dbContext) : IAccountRepository
{
    public async Task CreateAsync(Account account, CancellationToken cancellationToken = default)
    {
        await dbContext.Accounts.AddAsync(account, cancellationToken);
    }

    public Task<bool> DomainDeniedAsync(string topLevel, string secondLevel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EmailDeniedAsync(string email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts.AnyAsync(x => x.Email.Address.Equals(email), cancellationToken);
    }

    public Task<Account?> GetByDocumentAsync(string document, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts.FirstOrDefaultAsync(x => x.Email.Address.Equals(email), cancellationToken);
    }

    public async Task<Account?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts.FirstOrDefaultAsync(x => x.PublicId.Equals(publicId), cancellationToken);
    }
}