using System;

namespace Semogly.Core.Application.SharedContext.Events;

public abstract class IntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
