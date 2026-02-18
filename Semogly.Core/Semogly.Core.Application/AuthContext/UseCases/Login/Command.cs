using Semogly.Core.Application.AccountContext.UseCases.Login;
using Semogly.Core.Application.SharedContext.UseCases.Abstractions;

namespace Semogly.Core.Application.AuthContext.UseCases.Login;

public sealed record Command(string Email, string Password) : ICommand<Response>;
