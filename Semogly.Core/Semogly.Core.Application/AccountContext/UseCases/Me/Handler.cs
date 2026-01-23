using Semogly.Core.Application.SharedContext.Services;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.AccountContext.Errors;
using Semogly.Core.Domain.AccountContext.Repositories.Abstractions;
using Semogly.Core.Domain.SharedContext.Results;

namespace Semogly.Core.Application.AccountContext.UseCases.Me;

public class Handler(
    ICurrentSessionService currentSessionService,
    IAccountRepository accountRepository
) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        var userId = currentSessionService.UserId ?? Guid.Empty;
        if (userId == Guid.Empty)
            return Result.Failure<Response>(DomainErrors.Account.NotFound);

        var account = await accountRepository.GetByPublicIdAsync(userId, cancellationToken);

        if (account is null)
            return Result.Failure<Response>(DomainErrors.Account.NotFound);

        return Result.Success<Response>(account);
    }
}
