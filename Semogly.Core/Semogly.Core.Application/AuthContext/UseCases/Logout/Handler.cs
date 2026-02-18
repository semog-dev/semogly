using System;
using Semogly.Core.Application.SharedContext.Services;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.Shared.Primitives;

namespace Semogly.Core.Application.AccountContext.UseCases.Logout;

public class Handler(ICurrentSessionService currentSessionService) : ICommandHandler<Command, Response>
{
    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        currentSessionService.Logout();
        return Result.Success(new Response());
    }
}
