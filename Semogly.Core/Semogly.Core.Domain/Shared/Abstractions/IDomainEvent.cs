using MediatR;

namespace Semogly.Core.Domain.Shared.Abstractions;

public interface IDomainEvent : INotification
{
    DateTime OccurredOnUtc { get; }
}