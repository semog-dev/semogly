using System;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.AccountContext.Entities;

namespace Semogly.Core.Application.AccountContext.UseCases.Me;

public record Response(Guid PublicId, string Name, string Email) : ICommandResponse
{
    public static implicit operator Response(Account account) =>
        new(account.PublicId, account.Name, account.Email);
}
