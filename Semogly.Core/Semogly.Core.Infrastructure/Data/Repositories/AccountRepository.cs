using Microsoft.EntityFrameworkCore;
using Semogly.Core.Domain.AccountContext.Entities;
using Semogly.Core.Domain.AccountContext.Repositories.Abstractions;

namespace Semogly.Core.Infrastructure.Data.Repositories;

public class AccountRepository(SemoglyDbContext dbContext) : Repository<Account, int>(dbContext), IAccountRepository
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



}