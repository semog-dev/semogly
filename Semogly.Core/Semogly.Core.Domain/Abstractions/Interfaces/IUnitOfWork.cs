namespace Semogly.Core.Domain.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync();
}