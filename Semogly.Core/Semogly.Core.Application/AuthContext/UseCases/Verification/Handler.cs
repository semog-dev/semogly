using Semogly.Core.Application.SharedContext.Abstractions.Persistence;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.Shared.Abstractions;
using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Primitives;

namespace Semogly.Core.Application.AuthContext.UseCases.Verification;

public class Handler(
    IDateTimeProvider dateTimeProvider,
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByPublicIdAsync(request.PublicId, cancellationToken);

        if (account == null)
            return Result.Failure<Response>(DomainErrors.Account.NotFound);

        account.VerificationCode.Verify(request.VerificationCode, dateTimeProvider);

        await unitOfWork.CommitAsync(cancellationToken);
        return Result.Success(new Response(account.PublicId));
    }
}
