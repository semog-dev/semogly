using RabbitMQ.Client;
using Semogly.Core.Api.SharedContext.Common;
using Semogly.Core.Domain.Shared.Abstractions;
using Semogly.Core.Domain.Shared.Common;
using Semogly.Core.Infrastructure;
using Semogly.Core.Infrastructure.Messaging;
using Semogly.Core.Infrastructure.Messaging.Interfaces;
using Semogly.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.AddConfiguration();
builder.Services.AddData();
builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
{
    HostName = "localhost",
});

builder.Services.AddSingleton<IRabbitMqPersistentConnection, RabbitMqPersistentConnection>();
builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

builder.Services.AddHostedService<RabbitMqConsumer>();
builder.Services.AddHostedService<OutboxProcessor>();

var host = builder.Build();
host.Run();
