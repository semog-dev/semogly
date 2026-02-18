using RabbitMQ.Client;

namespace Semogly.Core.Infrastructure.Messaging.Interfaces;

public interface IRabbitMqPersistentConnection : IAsyncDisposable
{
    bool IsConnected { get; }
    Task<bool> TryConnectAsync();
    Task<IChannel> CreateChannelAsync();
}
