using Microsoft.EntityFrameworkCore;
using Semogly.Core.Domain.AccountContext.Entities;
using Semogly.Core.Domain.AccountContext.Repositories.Abstractions;

namespace Semogly.Core.Data.AccountContext.Repositories;

public class AccountRepository(AccountDbContext dbContext) : IAccountRepository
{
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

    public Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task SaveAsync(Account account, CancellationToken cancellationToken = default)
    {
        await dbContext.Accounts.AddAsync(account, cancellationToken);
    }

    public void UpdateAsync(Account account, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}