using System;
using Semogly.Core.Application.AuthContext.Models;

namespace Semogly.Core.Application.AuthContext.Abstractions;

public interface ISessionStore
{
    Task<Session?> GetSession(Guid sessionId);
    Task SetSession(Session session);
    Task RevokeSessionAsync(Guid sessionId);
}
