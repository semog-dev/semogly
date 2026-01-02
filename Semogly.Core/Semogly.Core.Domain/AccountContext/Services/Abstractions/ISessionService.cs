using System;
using Semogly.Core.Domain.AccountContext.Models;

namespace Semogly.Core.Domain.AccountContext.Services.Abstractions;

public interface ISessionService
{
    Task<Session?> GetSession(Guid sessionId);
    Task SetSession(Session session);
    Task RevokeSessionAsync(Guid sessionId);
}
