using Semogly.Core.Domain.Shared.Abstractions;

namespace Semogly.Core.Domain.AccountContext.Events;

public sealed record OnAccountCreatedEvent(Guid PublicId, string Name, string Email, IDateTimeProvider DateTimeProvider) : IDomainEvent, IIntegrationEventConvertible
{
    public DateTime OccurredOnUtc => DateTimeProvider.UtcNow;

    public object ToIntegrationEvent()
        => new AccountCreatedIntegrationEvent(PublicId, Name, Email);
}