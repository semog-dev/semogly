using Semogly.Core.Domain.AccountContext.Entities;

namespace Semogly.Core.Application.SharedContext.Abstractions.Persistence;

public interface IAccountRepository
{
    Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Account?> GetByDocumentAsync(string document, CancellationToken cancellationToken = default);
    Task<Account?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default);
    Task CreateAsync(Account account, CancellationToken cancellationToken = default);
    Task<bool> EmailDeniedAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> DomainDeniedAsync(string topLevel, string secondLevel, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
}