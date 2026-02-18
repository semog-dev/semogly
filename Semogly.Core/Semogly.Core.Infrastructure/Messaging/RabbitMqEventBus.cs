using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using Semogly.Core.Application.SharedContext.Events;
using Semogly.Core.Infrastructure.Messaging.Interfaces;

namespace Semogly.Core.Infrastructure.Messaging;

public class RabbitMqEventBus(IRabbitMqPersistentConnection persistentConnection, ILogger<RabbitMqEventBus> logger) : IEventBus, IAsyncDisposable
{
    private readonly IRabbitMqPersistentConnection _persistentConnection = persistentConnection;
    private readonly ILogger<RabbitMqEventBus> _logger = logger;
    private const string ExchangeName = "semogly.events";
    private static readonly IAsyncPolicy _circuitBreakerPolicy = 
        Policy.Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );
    private readonly IAsyncPolicy _retryPolicy = 
        Policy.Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    public async Task PublishAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IntegrationEvent
    {
        var eventName = typeof(TEvent).Name;
        var body = JsonSerializer.SerializeToUtf8Bytes(@event);

        var combinedPolicy = Policy.WrapAsync(_circuitBreakerPolicy, _retryPolicy);
        try 
        {
            await combinedPolicy.ExecuteAsync(async () => 
            {
                await using var channel = await _persistentConnection.CreateChannelAsync();
                await channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true);

                var properties = new BasicProperties { Persistent = true };

                await channel.BasicPublishAsync(
                    exchange: ExchangeName,
                    routingKey: eventName,
                    mandatory: false,
                    basicProperties: properties,
                    body: body,
                    cancellationToken: cancellationToken);
                
                _logger.LogInformation("Evento {EventName} ({EventId}) publicado com sucesso.", eventName, @event.Id);
            });
        }
        catch (BrokenCircuitException)
        {
            _logger.LogCritical("Circuito aberto! RabbitMQ está inacessível. Pulando tentativas.");
            throw;
        }
    }

    public async Task PublishAsync(
        string eventName,
        string payload,
        CancellationToken cancellationToken = default)
    {
        var bytes = Encoding.UTF8.GetBytes(payload);
        var combinedPolicy = Policy.WrapAsync(_circuitBreakerPolicy, _retryPolicy);
        try 
        {
            await combinedPolicy.ExecuteAsync(async () => 
            {
                await using var channel = await _persistentConnection.CreateChannelAsync();
                await channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true);

                var properties = new BasicProperties { Persistent = true };

                await channel.BasicPublishAsync(
                    exchange: ExchangeName,
                    routingKey: eventName,
                    mandatory: false,
                    basicProperties: properties,
                    body: bytes,
                    cancellationToken: cancellationToken);
                
                _logger.LogInformation("Evento {EventName} publicado com sucesso.", eventName);
            });
        }
        catch (BrokenCircuitException)
        {
            _logger.LogCritical("Circuito aberto! RabbitMQ está inacessível. Pulando tentativas.");
            throw;
        }        
    }

    public async ValueTask DisposeAsync()
    {
        if (_persistentConnection != null)
            await _persistentConnection.DisposeAsync();
    }
}

