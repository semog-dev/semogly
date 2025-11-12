using Semogly.Core.Domain.AccountContext.Entities;
using Semogly.Core.Domain.SharedContext.Repositories.Abstractions;

namespace Semogly.Core.Domain.AccountContext.Repositories.Abstractions;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Account?> GetByDocumentAsync(string document, CancellationToken cancellationToken = default);
    Task SaveAsync(Account account, CancellationToken cancellationToken = default);
    void UpdateAsync(Account account, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailDeniedAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> DomainDeniedAsync(string topLevel, string secondLevel, CancellationToken cancellationToken = default);
}