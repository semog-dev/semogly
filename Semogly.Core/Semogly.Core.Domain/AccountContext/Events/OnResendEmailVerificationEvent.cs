using Semogly.Core.Domain.SharedContext.Events.Abstractions;

namespace Semogly.Core.Domain.AccountContext.Events;

public sealed record OnResendEmailVerificationEvent(Guid Id, string Name, string Email, string VerificationCode) : IDomainEvent;