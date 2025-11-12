using Semogly.Core.Application.SharedContext.UseCases.Abstractions;

namespace Semogly.Core.Application.AccountContext.UseCases.Login;

public sealed record Command(string Email, string Password) : ICommand<Response>;
