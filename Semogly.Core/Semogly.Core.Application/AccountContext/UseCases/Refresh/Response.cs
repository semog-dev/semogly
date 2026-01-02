using Semogly.Core.Application.SharedContext.UseCases.Abstractions;

namespace Semogly.Core.Application.AccountContext.UseCases.Refresh;

public sealed record Response(string AccessToken) : ICommandResponse;
