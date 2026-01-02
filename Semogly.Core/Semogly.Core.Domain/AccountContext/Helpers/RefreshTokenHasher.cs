using System;
using System.Security.Cryptography;
using System.Text;

namespace Semogly.Core.Domain.AccountContext.Helpers;

public class RefreshTokenHasher
{
    public static string Hash(string token, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hash);
    }
}
