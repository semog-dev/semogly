using Semogly.Core.Domain.SharedContext.Abstractions;
using Semogly.Core.Domain.SharedContext.Exceptions;

namespace Semogly.Core.Domain.SharedContext.ValueObjects;

public sealed record Tracker : ValueObject
{
    private Tracker()
    {
    }

    private Tracker(DateTime createdAtUtc)
    {
        CreatedAtUtc = createdAtUtc;
    }
    public static Tracker Create()
        => new(DateTime.UtcNow);

    public static Tracker Create(IDateTimeProvider dateTimeProvider)
        => new(dateTimeProvider.UtcNow);
    public DateTime CreatedAtUtc { get; }
    public DateTime? UpdatedAtUtc { get; private set; } = null;
    public void Update(IDateTimeProvider dateTimeProvider)
    {
        if (dateTimeProvider.UtcNow < CreatedAtUtc)
            throw new InvalidDateTimeProviderIsExpired("");
        
        UpdatedAtUtc = dateTimeProvider.UtcNow;
    }
}