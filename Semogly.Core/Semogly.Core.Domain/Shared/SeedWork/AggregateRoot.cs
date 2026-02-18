using Semogly.Core.Domain.Shared.Abstractions;

namespace Semogly.Core.Domain.Shared.SeedWork;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId: notnull 
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected AggregateRoot() : base() { }

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
