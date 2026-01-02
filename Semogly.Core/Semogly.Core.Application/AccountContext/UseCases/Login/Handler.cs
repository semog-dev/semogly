using Semogly.Core.Application.SharedContext.Services;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.AccountContext.Errors;
using Semogly.Core.Domain.AccountContext.Helpers;
using Semogly.Core.Domain.AccountContext.Models;
using Semogly.Core.Domain.AccountContext.Repositories.Abstractions;
using Semogly.Core.Domain.AccountContext.Services.Abstractions;
using Semogly.Core.Domain.AccountContext.ValueObjects;
using Semogly.Core.Domain.SharedContext;
using Semogly.Core.Domain.SharedContext.Results;

namespace Semogly.Core.Application.AccountContext.UseCases.Login;

public class Handler(
    IAccountRepository repository,
    ITokenService tokenService,
    ISessionService sessionService,
    ICurrentSessionService currentSessionService
    ) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Guid deviceId = currentSessionService.DeviceId ?? Guid.NewGuid();
        currentSessionService.SetDeviceIdCookie(deviceId);     

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

        var refreshTokenHashed = RefreshTokenHasher.Hash(refreshToken, Configuration.Security.RefreshTokenSecret);

        var session = new Session(
            account.PublicId, 
            refreshTokenHashed, 
            deviceId, 
            currentSessionService.UserAgent, 
            currentSessionService.Ip);

        await sessionService.SetSession(session);

        currentSessionService.SetRefreshTokenCookie(refreshToken);
        currentSessionService.SetSessionIdCookie(session.Id);
            
        var response = new Response(accessToken);
        return Result.Success(response);
    }
}
