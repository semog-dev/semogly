using Semogly.Core.Application.SharedContext.UseCases.Abstractions;

namespace Semogly.Core.Application.AuthContext.UseCases.Verification;

public sealed record Command(Guid PublicId, string VerificationCode) : ICommand<Response>;
public sealed record Payload(string VerificationCode);
