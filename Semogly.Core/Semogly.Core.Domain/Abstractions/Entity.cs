namespace Semogly.Core.Domain.Abstractions;

public abstract class Entity
{
    public Guid Id { get; init; } = Guid.NewGuid();
}