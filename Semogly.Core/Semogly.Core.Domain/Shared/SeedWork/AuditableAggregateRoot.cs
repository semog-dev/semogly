using Semogly.Core.Domain.Shared.Abstractions;

namespace Semogly.Core.Domain.Shared.SeedWork;

public class AuditableAggregateRoot<TId> : AggregateRoot<TId>
    where TId: notnull
{
    public DateTime CreatedAtUtc { get; protected set; }
    public DateTime? UpdatedAtUtc { get; protected set; }

    protected AuditableAggregateRoot() { }

    protected AuditableAggregateRoot(IDateTimeProvider dateTimeProvider)
        : base()
    {
        CreatedAtUtc = dateTimeProvider.UtcNow;
    }

    public void SetUpdated(IDateTimeProvider dateTimeProvider)
    {
        UpdatedAtUtc = dateTimeProvider.UtcNow;
    }
}
