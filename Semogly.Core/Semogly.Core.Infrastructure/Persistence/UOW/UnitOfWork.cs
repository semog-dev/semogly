using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Text.Json;
using MediatR;
using Semogly.Core.Application.SharedContext.Abstractions.Persistence;
using Semogly.Core.Domain.Shared.Abstractions;
using Semogly.Core.Infrastructure.Outbox;

namespace Semogly.Core.Infrastructure.Persistence.UOW;

public class UnitOfWork(SemoglyDbContext dbContext, IPublisher publisher, IDateTimeProvider dateTimeProvider) : IUnitOfWork
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        var entities = dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();        

        foreach (var entity in entities)
                    entity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            if (domainEvent is IIntegrationEventConvertible convertible)
            {
                var integrationEvent = convertible.ToIntegrationEvent();

                var outboxMessage = new OutboxMessage(
                    integrationEvent.GetType().Name,
                    JsonSerializer.Serialize(integrationEvent, _jsonSerializerOptions),
                    dateTimeProvider);

                dbContext.OutboxMessages.Add(outboxMessage);
            }
        }

        foreach (var domainEvent in domainEvents)
            await publisher.Publish(domainEvent, cancellationToken);   

        await dbContext.SaveChangesAsync(cancellationToken);     
    }
}