using Semogly.Core.Domain.AccountContext.DTOs;

namespace Semogly.Core.Domain.AccountContext.Services.Abstractions;

public interface ITokenService
{
    Task<string?> GenerateAccessTokenAsync(string userId);
    Task<string?> GenerateRefreshTokenAsync(string userId);
    Task<string?> ValidateRefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
}
