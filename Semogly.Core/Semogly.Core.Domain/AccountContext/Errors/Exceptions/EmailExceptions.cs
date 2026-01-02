using Semogly.Core.Domain.SharedContext.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Errors.Exceptions;

public class EmailNullOrEmptyException() : DomainException(ErrorMessages.Email.NullOrEmpty);

public class InvalidEmailException() : DomainException(ErrorMessages.Email.Invalid);