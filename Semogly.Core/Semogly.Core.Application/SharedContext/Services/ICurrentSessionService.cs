using System;

namespace Semogly.Core.Application.SharedContext.Services;

public interface ICurrentSessionService
{
    Guid? SessionId { get; }
    Guid? UserId { get; }
    Guid? DeviceId { get; }
    string? UserAgent { get; }
    string? Ip { get; }
    string? RefreshToken { get; }
    string? AccessToken { get; }

    void SetAccessTokenCookie(string accessToken);
    void SetRefreshTokenCookie(string refreshToken);
    void SetSessionIdCookie(Guid sessionId);
    void SetDeviceIdCookie(Guid deviceId);
}
