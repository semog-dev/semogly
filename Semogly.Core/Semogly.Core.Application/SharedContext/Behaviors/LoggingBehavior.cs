using MediatR;
using Microsoft.Extensions.Logging;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;

namespace Semogly.Core.Application.SharedContext.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Handling request: {request.GetType().Name}");
            var result = await next(cancellationToken);
            logger.LogInformation($"Request {request.GetType().Name} processed");
            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while handling request: {request.GetType().Name}");
            throw;
        }
    }
}