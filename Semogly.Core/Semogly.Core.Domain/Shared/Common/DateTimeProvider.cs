using Semogly.Core.Domain.Shared.Abstractions;

namespace Semogly.Core.Domain.Shared.Common;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow { get; } = DateTime.UtcNow;
}