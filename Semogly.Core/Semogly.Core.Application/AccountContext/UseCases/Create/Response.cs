using Semogly.Core.Application.SharedContext.UseCases.Abstractions;

namespace Semogly.Core.Application.AccountContext.UseCases.Create;

public sealed record Response(Guid PublicId, string Name, string Email) : ICommandResponse;