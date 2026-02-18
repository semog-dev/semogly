using Semogly.Core.Domain.Shared.Abstractions;

namespace Semogly.Core.Domain.AccountContext.Events;

public sealed record OnResendEmailVerificationEvent(int Id, string Name, string Email, string VerificationCode, IDateTimeProvider DateTimeProvider) 
    : IDomainEvent
{
    public DateTime OccurredOnUtc => DateTimeProvider.UtcNow;
}