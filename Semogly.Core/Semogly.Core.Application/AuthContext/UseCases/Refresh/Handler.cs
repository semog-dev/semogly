using Semogly.Core.Application.AuthContext.Abstractions;
using Semogly.Core.Application.AuthContext.Models;
using Semogly.Core.Application.SharedContext.Abstractions.Persistence;
using Semogly.Core.Application.SharedContext.Services;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.Shared;
using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Primitives;

namespace Semogly.Core.Application.AccountContext.UseCases.Refresh;

public class Handler(
    ITokenService tokenService,
    ISessionStore sessionStore,
    IAccountRepository accountRepository,
    ICurrentSessionService currentSessionService
    ) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        if (!currentSessionService.SessionId.HasValue)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var session = await sessionStore.GetSession(currentSessionService.SessionId.Value);
        if (session is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        if (string.IsNullOrEmpty(currentSessionService.RefreshToken))
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var refreshTokenHashed = tokenService.HashToken(currentSessionService.RefreshToken, Configuration.Security.RefreshTokenSecret);

        if (session.RefreshTokenHashed != refreshTokenHashed)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var account = await accountRepository.GetByPublicIdAsync(session.UserId, cancellationToken);

        if (account is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        await sessionStore.RevokeSessionAsync(session.Id);

        var newAccessToken = await tokenService.GenerateAccessTokenAsync(account);
        if (newAccessToken is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var newRefreshToken = await tokenService.GenerateRefreshTokenAsync();
        if (newRefreshToken is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);
        var newRefreshTokenHashed = tokenService.HashToken(newRefreshToken, Configuration.Security.RefreshTokenSecret);

        var newSession = new Session(account.PublicId, newRefreshTokenHashed, session.DeviceId, currentSessionService.UserAgent ?? string.Empty, currentSessionService.Ip ?? string.Empty);

        await sessionStore.SetSession(newSession);

        currentSessionService.SetRefreshTokenCookie(newRefreshToken);
        currentSessionService.SetSessionIdCookie(newSession.Id);
        currentSessionService.SetAccessTokenCookie(newAccessToken);

        return Result.Success(new Response());
    }
}
