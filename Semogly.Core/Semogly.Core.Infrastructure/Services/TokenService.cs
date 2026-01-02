using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Semogly.Core.Domain.AccountContext.Entities;
using Semogly.Core.Domain.AccountContext.Services.Abstractions;
using Semogly.Core.Domain.SharedContext;

namespace Semogly.Core.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly byte[] _key = Encoding.UTF8.GetBytes(Configuration.Jwt.Key);

    public async Task<string?> GenerateAccessTokenAsync(Account account)
    {
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

    public async Task<string?> GenerateRefreshTokenAsync()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(bytes);
        return refreshToken;
    }

    // public static bool CompareRefreshTokenHashedAsync(string refreshToken, string storeRefreshTokenHashed)
    // {
    //     var refreshTokenHashed = Hash(refreshToken, Configuration.Security.RefreshTokenSecret);
    //     return refreshTokenHashed == storeRefreshTokenHashed;
    // }

    // private static string Hash(string token, string secret)
    // {
    //     using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
    //     var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
    //     return Convert.ToBase64String(hash);
    // }
}
