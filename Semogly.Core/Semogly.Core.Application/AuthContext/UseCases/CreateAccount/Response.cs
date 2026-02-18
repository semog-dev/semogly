using Semogly.Core.Application.SharedContext.UseCases.Abstractions;

namespace Semogly.Core.Application.AuthContext.UseCases.CreateAccount;

public sealed record Response(Guid PublicId, string Name, string Email) : ICommandResponse;