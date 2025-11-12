using Semogly.Core.Domain.SharedContext.Abstractions;

namespace Semogly.Core.Domain.SharedContext.Common;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow { get; } = DateTime.UtcNow;
}