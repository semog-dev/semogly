using Semogly.Core.Application.SharedContext.Abstractions.Persistence;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.AccountContext.Entities;
using Semogly.Core.Domain.AccountContext.ValueObjects;
using Semogly.Core.Domain.Shared.Abstractions;
using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Primitives;

namespace Semogly.Core.Application.AuthContext.UseCases.CreateAccount;

public sealed class Handler(
    IDateTimeProvider dateTimeProvider,
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        // Verifica se o E-mail já está cadastrado
        var emailExists = await accountRepository.ExistsAsync(request.Email, cancellationToken);
        if (emailExists)
            return Result.Failure<Response>(DomainErrors.Account.EmailInUse);

        // Gera os VOs
        var name = CompositeName.Create(request.FirstName, null, request.LastName);
        var email = Email.Create(request.Email);
        var password = Password.Create(request.Password);

        // Gera a entidade
        var account = Account.CreateAccount(name, email, password, dateTimeProvider);

        // Persiste os dados
        await accountRepository.CreateAsync(account, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        var response = new Response(account.PublicId, account.Name, account.Email);
        return Result.Success(response);
    }
}