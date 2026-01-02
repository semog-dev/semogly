using Semogly.Core.Application.SharedContext.Services;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.AccountContext.Errors;
using Semogly.Core.Domain.AccountContext.Helpers;
using Semogly.Core.Domain.AccountContext.Models;
using Semogly.Core.Domain.AccountContext.Repositories.Abstractions;
using Semogly.Core.Domain.AccountContext.Services.Abstractions;
using Semogly.Core.Domain.SharedContext;
using Semogly.Core.Domain.SharedContext.Results;

namespace Semogly.Core.Application.AccountContext.UseCases.Refresh;

public class Handler(
    ITokenService tokenService,
    ISessionService sessionService,
    IAccountRepository accountRepository,
    ICurrentSessionService currentSessionService
    ) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        var session = await sessionService.GetSession(currentSessionService.SessionId);
        if (session is null)
             return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var refreshTokenHashed = RefreshTokenHasher.Hash(currentSessionService.RefreshToken, Configuration.Security.RefreshTokenSecret);

        if (session.RefreshTokenHashed != refreshTokenHashed)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var account = await accountRepository.GetByPublicIdAsync(session.UserId, cancellationToken);

        if (account is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        await sessionService.RevokeSessionAsync(session.Id);

        var newAccessToken = await tokenService.GenerateAccessTokenAsync(account);
        if (newAccessToken is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var newRefreshToken = await tokenService.GenerateRefreshTokenAsync();
        if (newRefreshToken is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);
        var newRefreshTokenHashed = RefreshTokenHasher.Hash(newRefreshToken, Configuration.Security.RefreshTokenSecret);

        var newSession = new Session(account.PublicId, newRefreshTokenHashed, session.DeviceId, currentSessionService.UserAgent, currentSessionService.Ip);

        await sessionService.SetSession(newSession);

        currentSessionService.SetRefreshTokenCookie(newRefreshToken);
        currentSessionService.SetSessionIdCookie(newSession.Id);

        return Result.Success(new Response(newAccessToken));
    }
}
