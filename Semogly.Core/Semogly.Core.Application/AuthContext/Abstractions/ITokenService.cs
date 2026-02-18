using Semogly.Core.Domain.AccountContext.Entities;

namespace Semogly.Core.Application.AuthContext.Abstractions;

public interface ITokenService
{
    Task<string?> GenerateAccessTokenAsync(Account account);
    Task<string?> GenerateRefreshTokenAsync();
    string HashToken(string token, string secret);
}
