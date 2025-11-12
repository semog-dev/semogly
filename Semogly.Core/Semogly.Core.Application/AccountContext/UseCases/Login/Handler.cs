using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.AccountContext.Errors;
using Semogly.Core.Domain.AccountContext.Repositories.Abstractions;
using Semogly.Core.Domain.AccountContext.Services.Abstractions;
using Semogly.Core.Domain.AccountContext.ValueObjects;
using Semogly.Core.Domain.SharedContext.Results;

namespace Semogly.Core.Application.AccountContext.UseCases.Login;

public class Handler(
    IAccountRepository repository,
    IJwtService jwtService
    ) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByEmailAsync(request.Email, cancellationToken);

        if (account is null)
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        if (!Password.Verify(account.Password.HashText, request.Password))
            return Result.Failure<Response>(DomainErrors.Account.PasswordIsInvalid);

        var accessToken = await jwtService.GenerateToken(new(account.Id, account.Email.Address, account.Name));
            
        var response = new Response(accessToken);
        return Result.Success(response);
    }
}
