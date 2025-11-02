using System.Linq.Expressions;

namespace Semogly.Core.Domain.Abstractions.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
    bool IsSatisfiedBy(T entity);
}