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

builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
{
    HostName = Environment.GetEnvironmentVariable("RabbitMQ__ConnectionString") ?? string.Empty,
});

builder.Services.AddSingleton<IRabbitMqPersistentConnection, RabbitMqPersistentConnection>();
builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

builder.Services.AddScoped<IEmailService, MailgunService>();

builder.Services.AddHostedService<RabbitMqConsumer>();
builder.Services.AddHostedService<OutboxProcessor>();

var host = builder.Build();
host.Run();
