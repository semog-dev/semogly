using System;
using Semogly.Core.Application.SharedContext.Services;

namespace Semogly.Core.Api.SharedContext.Services;

public class CurrentSessionService(IHttpContextAccessor httpContextAccessor) : ICurrentSessionService
{
    private readonly HttpContext _context = httpContextAccessor.HttpContext 
        ?? throw new Exception("Erro ao instanciar HttpContext");

    public Guid? SessionId
    {
        get
        {
            if (!_context.Request.Cookies.TryGetValue("sessionId", out var sessionId))
                return null;

            return Guid.TryParse(sessionId, out var guid)
                ? guid
                : null;
        }
    }

    public Guid? UserId =>
        Guid.TryParse(_context.User?.FindFirst("userId")?.Value, out Guid guid)
            ? guid
            : null;

    public Guid? DeviceId
    {
        get
        {
            if (!_context.Request.Cookies.TryGetValue("deviceId", out var deviceId))
                return null;

            return Guid.TryParse(deviceId, out var guid)
                ? guid
                : null;
        }
    }

    public string? UserAgent => _context.Request.Headers.UserAgent.ToString();

    public string? Ip => _context.Connection.RemoteIpAddress?.ToString();

    public string? RefreshToken
    {
        get
        {
            if (!_context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return null;

            return refreshToken;
        }
    }

    public void SetDeviceIdCookie(Guid deviceId)
    {
        _context.Response.Cookies.Append(
        "deviceId",
        deviceId.ToString(),
        new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            Path = "/"
        });
    }

    public void SetRefreshTokenCookie(string refreshToken)
    {
        _context.Response.Cookies.Append(
        "refreshToken",
        refreshToken,
        new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Path = "/"
        });
    }

    public void SetSessionIdCookie(Guid sessionId)
    {
        _context.Response.Cookies.Append(
        "sessionId",
        sessionId.ToString(),
        new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Path = "/"
        });
    }
}
