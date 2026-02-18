using Semogly.Core.Application.AccountContext.UseCases.Login;
using Semogly.Core.Application.AuthContext.Abstractions;
using Semogly.Core.Application.AuthContext.Models;
using Semogly.Core.Application.SharedContext.Abstractions.Persistence;
using Semogly.Core.Application.SharedContext.Services;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.AccountContext.ValueObjects;
using Semogly.Core.Domain.Shared;
using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Primitives;

namespace Semogly.Core.Application.AuthContext.UseCases.Login;

public class Handler(
    IAccountRepository repository,
    ITokenService tokenService,
    ISessionStore sessionStore,
    ICurrentSessionService currentSessionService
    ) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        var newDeviceId = Guid.NewGuid();
        if (!currentSessionService.DeviceId.HasValue)
            currentSessionService.SetDeviceIdCookie(newDeviceId);     

        var account = await repository.GetByEmailAsync(request.Email, cancellationToken);

        if (account is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        if (!Password.Verify(account.Password.HashText, request.Password))
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var accessToken = await tokenService.GenerateAccessTokenAsync(account);

        if (accessToken is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var refreshToken = await tokenService.GenerateRefreshTokenAsync();

        if (refreshToken is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var refreshTokenHashed = tokenService.HashToken(refreshToken, Configuration.Security.RefreshTokenSecret);

        var session = new Session(
            account.PublicId, 
            refreshTokenHashed, 
            currentSessionService.DeviceId ?? newDeviceId, 
            currentSessionService.UserAgent ?? string.Empty, 
            currentSessionService.Ip ?? string.Empty);

        await sessionStore.SetSession(session);

        currentSessionService.SetRefreshTokenCookie(refreshToken);
        currentSessionService.SetSessionIdCookie(session.Id);
        currentSessionService.SetAccessTokenCookie(accessToken);

        return Result.Success(new Response());
    }
}
