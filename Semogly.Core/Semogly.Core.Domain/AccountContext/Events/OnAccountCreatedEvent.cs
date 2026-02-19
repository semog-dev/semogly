using Semogly.Core.Domain.Shared.Abstractions;

namespace Semogly.Core.Domain.AccountContext.Events;

public sealed record OnAccountCreatedEvent(Guid PublicId, string Name, string Email, string VerificationCode, IDateTimeProvider DateTimeProvider) : IDomainEvent, IIntegrationEventConvertible
{
    public DateTime OccurredOnUtc => DateTimeProvider.UtcNow;

    public object ToIntegrationEvent()
        => new AccountCreatedIntegrationEvent(PublicId, Name, Email, VerificationCode);
}