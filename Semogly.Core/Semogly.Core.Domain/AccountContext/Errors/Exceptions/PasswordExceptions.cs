using System;
using Semogly.Core.Domain.SharedContext.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Errors.Exceptions;

public class InvalidPasswordException() : DomainException(ErrorMessages.Password.Invalid);
public class InvalidPasswordLengthException() : DomainException(ErrorMessages.Password.Invalid);
public class PasswordNullException() : DomainException(ErrorMessages.Password.Invalid);