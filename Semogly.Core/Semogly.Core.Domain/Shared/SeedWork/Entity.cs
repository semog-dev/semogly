namespace Semogly.Core.Domain.Shared.SeedWork;

public abstract class Entity<TId> : IEquatable<TId>
    where TId : notnull
{
    public TId Id { get; init; } = default!;
    public Guid PublicId { get; protected set; } = Guid.NewGuid();

    protected Entity() 
    {
        // Constructor required by EF Core
    }


    public bool Equals(TId? id)
        => Id.Equals(id);

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();
}
