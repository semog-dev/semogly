namespace Semogly.Core.Domain.AccountContext.Models;

public class Session
{
    public Session(Guid userId, string refreshTokenHashed, string userAgent, string ip)
    {
        UserId = userId;
        RefreshTokenHashed = refreshTokenHashed;
        UserAgent = userAgent;
        Ip = ip;
    }

    public Session(Guid userId, string refreshTokenHashed, Guid deviceId, string userAgent, string ip)
    {
        UserId = userId;
        RefreshTokenHashed = refreshTokenHashed;
        DeviceId = deviceId;
        UserAgent = userAgent;
        Ip = ip;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string RefreshTokenHashed { get; set; } = string.Empty;
    public Guid DeviceId { get; set; } = Guid.NewGuid();
    public string UserAgent { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActivity { get; set; } = DateTime.UtcNow;
    public int Ttl { get; set; } = 7;
}
