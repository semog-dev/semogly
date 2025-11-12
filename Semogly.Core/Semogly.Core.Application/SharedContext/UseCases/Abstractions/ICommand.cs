using MediatR;
using Semogly.Core.Domain.SharedContext.Results;

namespace Semogly.Core.Application.SharedContext.UseCases.Abstractions;

public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TResult> : IRequest<Result<TResult>>, IBaseCommand where TResult : ICommandResponse;

public interface IBaseCommand
{
}