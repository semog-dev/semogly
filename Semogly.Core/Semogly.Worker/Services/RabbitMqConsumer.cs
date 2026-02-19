using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Semogly.Core.Domain.AccountContext.Events;
using Semogly.Core.Domain.Shared;
using Semogly.Core.Infrastructure.Mail.Interfaces;
using Semogly.Core.Infrastructure.Mail.Models;
using Semogly.Core.Infrastructure.Messaging.Interfaces;

namespace Semogly.Worker.Services;

public class RabbitMqConsumer(
    IRabbitMqPersistentConnection persistentConnection,
    IServiceScopeFactory scopeFactory,
    ILogger<RabbitMqConsumer> logger,
    IEmailService emailService) : BackgroundService
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

                var accountCreatedIntegrationEvent = JsonSerializer.Deserialize<AccountCreatedIntegrationEvent>(body)
                    ?? throw new Exception();
                
                logger.LogInformation("Processando mensagem {Id}", messageId);

                var variables = new Dictionary<string, object>
                {
                    {"Link", $"{Configuration.Environment.FrontBaseUrl}auth/account-verification/{accountCreatedIntegrationEvent.AccountPublicId}?verificationCode={accountCreatedIntegrationEvent.VerificationCode}"}
                };

                var emailMessage = new EmailMessage(
                    "Account Confirmation", 
                    Configuration.Mailgun.From, 
                    "account-confirmation", 
                    variables, 
                    [accountCreatedIntegrationEvent.Email]);

                await emailService.Send(emailMessage);

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
