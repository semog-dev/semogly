using Semogly.Core.Domain.SharedContext.Aggregates.Abstractions;

namespace Semogly.Core.Domain.SharedContext.Repositories.Abstractions;

public interface IRepository<TEntity> where TEntity : IAggregateRoot;