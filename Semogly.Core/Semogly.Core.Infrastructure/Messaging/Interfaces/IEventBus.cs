using Semogly.Core.Application.SharedContext.Events;

namespace Semogly.Core.Infrastructure.Messaging.Interfaces;

public interface IEventBus
{
    Task PublishAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IntegrationEvent;

    Task PublishAsync(
        string eventName,
        string payload,
        CancellationToken cancellationToken = default);
}