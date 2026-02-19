using Semogly.Core.Application.SharedContext.UseCases.Abstractions;

namespace Semogly.Core.Application.AuthContext.UseCases.Verification;

public record Response(Guid PublicId) : ICommandResponse;
