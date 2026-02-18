using Semogly.Core.Application.SharedContext.Abstractions.Persistence;
using Semogly.Core.Application.SharedContext.Services;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Primitives;

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
