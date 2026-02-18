using RabbitMQ.Client;
using Semogly.Core.Infrastructure.Messaging.Interfaces;

namespace Semogly.Core.Infrastructure.Messaging;

public class RabbitMqPersistentConnection(IConnectionFactory connectionFactory) : IRabbitMqPersistentConnection
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;
    private IConnection? _connection;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);

    public bool IsConnected => _connection is { IsOpen: true };

    public async Task<IChannel> CreateChannelAsync()
    {
        if (!IsConnected) await TryConnectAsync();
        return await _connection!.CreateChannelAsync();
    }

    public async Task<bool> TryConnectAsync()
    {
        await _connectionLock.WaitAsync();
        try
        {
            if (IsConnected) return true;
            _connection = await _connectionFactory.CreateConnectionAsync();
            return true;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public async ValueTask DisposeAsync() => await (_connection?.DisposeAsync() ?? ValueTask.CompletedTask);
}
