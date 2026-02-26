using RabbitMQ.Client;
using Semogly.Core.Domain.Shared.Abstractions;
using Semogly.Core.Domain.Shared.Common;
using Semogly.Core.Infrastructure;
using Semogly.Core.Infrastructure.Mail.Interfaces;
using Semogly.Core.Infrastructure.Mail.Services;
using Semogly.Core.Infrastructure.Messaging;
using Semogly.Core.Infrastructure.Messaging.Interfaces;
using Semogly.Worker;
using Semogly.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.AddConfiguration();
builder.Services.AddData();
builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddSingleton<IConnectionFactory>(sp => 
{
    var connectionString = Environment.GetEnvironmentVariable("RabbitMQ__ConnectionString");
    
    var factory = new ConnectionFactory();

    if (!string.IsNullOrEmpty(connectionString))
    {
        // Se for uma URI (amqp://...), usamos a propriedade Uri
        factory.Uri = new Uri(connectionString);
    }
    else
    {
        // Fallback para localhost caso a variável não exista
        factory.HostName = "localhost";
    }

    // Importante para o seu AsyncEventingBasicConsumer funcionar corretamente
    // factory.DispatchConsumersAsync = true; 

    return factory;
});

builder.Services.AddSingleton<IRabbitMqPersistentConnection, RabbitMqPersistentConnection>();
builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

builder.Services.AddScoped<IEmailService, MailgunService>();

builder.Services.AddHostedService<RabbitMqConsumer>();
builder.Services.AddHostedService<OutboxProcessor>();

var host = builder.Build();
host.Run();
