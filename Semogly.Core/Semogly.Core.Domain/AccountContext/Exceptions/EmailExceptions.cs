using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Exceptions;

public class EmailNullOrEmptyException() : DomainException(ErrorMessages.Email.NullOrEmpty);

public class InvalidEmailException() : DomainException(ErrorMessages.Email.Invalid);