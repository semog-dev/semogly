using Semogly.Core.Domain.SharedContext.Events.Abstractions;

namespace Semogly.Core.Domain.AccountContext.Events;

public sealed record OnAccountCreatedEvent(Guid Id, string Name, string Email) : IDomainEvent;