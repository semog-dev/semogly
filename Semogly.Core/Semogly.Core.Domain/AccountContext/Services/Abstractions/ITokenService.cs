using Semogly.Core.Domain.AccountContext.DTOs;
using Semogly.Core.Domain.AccountContext.Entities;

namespace Semogly.Core.Domain.AccountContext.Services.Abstractions;

public interface ITokenService
{
    Task<string?> GenerateAccessTokenAsync(Account account);
    Task<string?> GenerateRefreshTokenAsync();
}
