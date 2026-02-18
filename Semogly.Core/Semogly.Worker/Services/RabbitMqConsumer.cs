using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Semogly.Core.Infrastructure.Messaging.Interfaces;

namespace Semogly.Worker.Services;

public class RabbitMqConsumer(
    IRabbitMqPersistentConnection persistentConnection,
    IServiceScopeFactory scopeFactory,
    ILogger<RabbitMqConsumer> logger) : BackgroundService
{
    private IChannel? _channel;
    private const string QueueName = "account-created-queue";

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _channel = await persistentConnection.CreateChannelAsync();
        await ConfigureInfrastructureAsync(ct);
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            try
            {
                var messageId = ea.BasicProperties.MessageId ?? string.Empty;
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                
                logger.LogInformation("Processando mensagem {Id}", messageId);

                // 4. Lógica de Negócio e Idempotência

                // await ProcessEventAsync(scope, messageId, body, ct);

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha crítica no processamento.");

                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, ct);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, ct);
    }

    private async Task ConfigureInfrastructureAsync(CancellationToken ct)
    {
        await _channel!.ExchangeDeclareAsync("semogly.events", ExchangeType.Topic, durable: true, cancellationToken: ct);
        await _channel!.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);
        await _channel.QueueBindAsync(QueueName, "semogly.events", "AccountCreatedIntegrationEvent", cancellationToken: ct);
    }
}
