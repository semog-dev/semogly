using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Exceptions;

public class InvalidPasswordException() : DomainException(ErrorMessages.Password.Invalid);
public class InvalidPasswordLengthException() : DomainException(ErrorMessages.Password.Invalid);
public class PasswordNullException() : DomainException(ErrorMessages.Password.Invalid);