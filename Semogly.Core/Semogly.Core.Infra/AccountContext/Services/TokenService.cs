using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Semogly.Core.Domain.AccountContext.Repositories.Abstractions;
using Semogly.Core.Domain.AccountContext.Services.Abstractions;
using Semogly.Core.Domain.SharedContext;
using StackExchange.Redis;

namespace Semogly.Core.Infra.AccountContext.Services;

public class TokenService(IAccountRepository accountRepository, IConnectionMultiplexer connection) : ITokenService
{
    private readonly byte[] _key = Encoding.UTF8.GetBytes(Configuration.Jwt.Key);
    private readonly IDatabase _redis = connection.GetDatabase();

    public async Task<string?> GenerateAccessTokenAsync(string userId)
    {
        if (!Guid.TryParse(userId, out var accountId))
            return null;

        var account = await accountRepository.GetByIdAsync(accountId);

        if (account is null)
            return null;

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new (JwtRegisteredClaimNames.Email, account.Email),
            new ("name", account.Name ?? string.Empty)
            // adicione outros claims necessários, ex: roles, tenant, etc.
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(Configuration.Jwt.AccessTokenExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: Configuration.Jwt.Issuer,
            audience: Configuration.Jwt.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public async Task<string?> GenerateRefreshTokenAsync(string userId)
    {
        var refreshToken = Guid.NewGuid().ToString("N");
        var expiration = TimeSpan.FromDays(7);

        await _redis.StringSetAsync($"{Configuration.Redis.RefreshTokenPrefix}{refreshToken}", userId, expiration);
        return refreshToken;
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        await _redis.KeyDeleteAsync($"{Configuration.Redis.RefreshTokenPrefix}{refreshToken}");
    }

    public async Task<string?> ValidateRefreshTokenAsync(string refreshToken)
    {
        var userId = await _redis.StringGetAsync($"{Configuration.Redis.RefreshTokenPrefix}{refreshToken}");
        return userId.HasValue ? userId.ToString() : null;
    }
}
