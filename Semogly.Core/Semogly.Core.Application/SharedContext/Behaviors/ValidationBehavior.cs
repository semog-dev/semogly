using FluentValidation;
using MediatR;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;
using Semogly.Core.Domain.SharedContext.Results;

namespace Semogly.Core.Application.SharedContext.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validators.Any() == false)
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToList()
            );

        if (validationErrors.Count > 0)
        {
            // Cria um Error composto com mensagens de validação
            var error = Error.Validation(
                "Validation.Failed",
                "Uma ou mais validações falharam.",
                validationErrors
            );

            // Retorna um Result.Failure
            // Necessário que TResponse seja um Result ou Result<T>
            if (typeof(TResponse) == typeof(Result))
                return (TResponse)(object)Result.Failure(error);

            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var innerType = typeof(TResponse).GetGenericArguments()[0];

                // Pega o método genérico Failure<T>(Error error)
                var method = typeof(Result)
                    .GetMethods()
                    .FirstOrDefault(m =>
                        m.Name == nameof(Result.Failure) &&
                        m.IsGenericMethodDefinition &&
                        m.GetGenericArguments().Length == 1);

                if (method is not null)
                {
                    var genericMethod = method.MakeGenericMethod(innerType);
                    var result = genericMethod.Invoke(null, new object[] { error })!;
                    return (TResponse)result;
                }
            }

            // Se TResponse não for um Result, apenas segue o fluxo normal
            return await next(cancellationToken);
        }

        return await next(cancellationToken);
    }
}