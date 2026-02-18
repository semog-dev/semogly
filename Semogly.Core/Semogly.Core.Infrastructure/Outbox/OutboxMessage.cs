using Semogly.Core.Domain.Shared.Abstractions;
using Semogly.Core.Domain.Shared.SeedWork;

namespace Semogly.Core.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    private OutboxMessage()
    {
        
    }

    public OutboxMessage(string type, string content, IDateTimeProvider dateTimeProvider)
    {
        Id = Guid.NewGuid();
        Type = type;
        Content = content;
        OccurredOnUtc = dateTimeProvider.UtcNow;
    }


    public Guid Id { get; init; }
    public string Type { get; private set; }
    public string Content { get; private set; }
    public DateTime OccurredOnUtc { get; private set; }
    public DateTime? ProcessedOnUtc { get; private set; }
    public string? Error { get; private set; }

    public void MarkAsProcessed(IDateTimeProvider dateTimeProvider)
    {
        ProcessedOnUtc = dateTimeProvider.UtcNow;
    }

    public void MarkAsFailed(string error)
    {
        Error = error;
    }
}
