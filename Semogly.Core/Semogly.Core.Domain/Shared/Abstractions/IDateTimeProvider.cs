namespace Semogly.Core.Domain.Shared.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}