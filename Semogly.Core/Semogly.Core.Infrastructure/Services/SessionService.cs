using System;
using Semogly.Core.Domain.AccountContext.Models;
using Semogly.Core.Domain.AccountContext.Services.Abstractions;
using StackExchange.Redis;

namespace Semogly.Core.Infrastructure.Services;

public class SessionService(IConnectionMultiplexer connection) : ISessionService
{
    private readonly IDatabase _redis = connection.GetDatabase();

    public async Task<Session?> GetSession(Guid sessionId)
    {
        var sessionKey = $"session:{sessionId}";
        var entries = await _redis.HashGetAllAsync(sessionKey);

        if (entries.Length == 0)
            return null;

        var data = entries.ToDictionary(
            x => x.Name.ToString(),
            x => x.Value
        );

        var session = new Session(
            GetGuid(data, "userId"), 
            GetString(data, "refreshTokenHashed"), 
            GetGuid(data, "deviceId"), 
            GetString(data, "userAgent"), 
            GetString(data, "ip")
            )
        {
            Id = sessionId,
            CreatedAt = GetDateTime(data, "createdAt"),
            LastActivity = GetDateTime(data, "lastActivity")
        };

        return session;
    }
    public async Task SetSession(Session session)
    {
        await _redis.HashSetAsync($"session:{session.Id}",
        [
            new("userId", $"{session.UserId}"),
            new("refreshTokenHashed", session.RefreshTokenHashed),
            new("deviceId", $"{session.DeviceId}"),
            new("userAgent", session.UserAgent),
            new("ip", session.Ip),
            new("createdAt", DateTime.UtcNow.ToString("O")),
            new("lastActivity", DateTime.UtcNow.ToString("O"))
        ]);

        await _redis.KeyExpireAsync(
            $"session:{session.Id}",
            TimeSpan.FromDays(7)
        );
    }

    public async Task RevokeSessionAsync(Guid sessionId)
    {
        var sessionKey = $"session:{sessionId}";
        await _redis.KeyDeleteAsync(sessionKey);
    }

    private static Guid GetGuid(
        Dictionary<string, RedisValue> data,
        string key)
    {
        return data.TryGetValue(key, out var value) &&
            value.HasValue &&
            Guid.TryParse(value.ToString(), out var guid)
            ? guid
            : Guid.Empty;
    }

    private static string GetString(
        Dictionary<string, RedisValue> data,
        string key)
    {
        return data.TryGetValue(key, out var value) && value.HasValue
            ? value.ToString()
            : string.Empty;
    }

    private static DateTime GetDateTime(
        Dictionary<string, RedisValue> data,
        string key)
    {
        return data.TryGetValue(key, out var value) &&
            value.HasValue &&
            long.TryParse(value.ToString(), out var ticks)
            ? DateTime.FromBinary(ticks)
            : DateTime.UtcNow;
    }

    
}
