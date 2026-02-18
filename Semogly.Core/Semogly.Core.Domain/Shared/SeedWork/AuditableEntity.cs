using Semogly.Core.Domain.Shared.Abstractions;

namespace Semogly.Core.Domain.Shared.SeedWork;

public abstract class AuditableEntity<TId> : Entity<TId>
    where TId: notnull
{
    public DateTime CreatedAtUtc { get; protected set; }
    public DateTime? UpdatedAtUtc { get; protected set; }

    protected AuditableEntity() : base()
    {
        
    }

    protected AuditableEntity(IDateTimeProvider dateTimeProvider)
        : base()
    {
        CreatedAtUtc = dateTimeProvider.UtcNow;
    }

    protected void SetUpdated(DateTime utcNow)
    {
        UpdatedAtUtc = utcNow;
    }

}
