using System.Linq.Expressions;
using Semogly.Core.Domain.Abstractions.Interfaces;

namespace Semogly.Core.Domain.Abstractions;

public abstract class Specification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }
}