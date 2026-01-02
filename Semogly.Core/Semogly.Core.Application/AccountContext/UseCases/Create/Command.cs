using Semogly.Core.Application.SharedContext.UseCases.Abstractions;

namespace Semogly.Core.Application.AccountContext.UseCases.Create;

public sealed record Command(string FirstName, string LastName, string Email, string Password) : ICommand<Response>;