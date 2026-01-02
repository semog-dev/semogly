using Semogly.Core.Domain.SharedContext.Events.Abstractions;
using Semogly.Core.Domain.SharedContext.ValueObjects;

namespace Semogly.Core.Domain.SharedContext.Entities;

public abstract class Entity<TKey> : IEquatable<TKey>
    where TKey : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public TKey Id { get; protected set; } = default!;
    public Guid PublicId { get; protected set; } = Guid.NewGuid();
    public Tracker Tracker { get; protected set; } = null!;

    protected Entity() 
    {
        // Constructor required by EF Core
    }

    protected Entity(Tracker tracker)
    {
        Tracker = tracker;
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents;

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);

    public bool Equals(TKey? id)
        => Id.Equals(id);

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TKey> other)
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();
}
