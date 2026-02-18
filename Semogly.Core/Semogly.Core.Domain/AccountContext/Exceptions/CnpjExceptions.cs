using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Exceptions;

public class CnpjNullOrEmptyException() : DomainException(ErrorMessages.Cnpj.NullOrEmpty);
public class InvalidCnpjException() : DomainException(ErrorMessages.Cnpj.Invalid);
public class InvalidCnpjLenghtException() : DomainException(ErrorMessages.Cnpj.InvalidLength);
public class InvalidCnpjNumberException() : DomainException(ErrorMessages.Cnpj.InvalidNumber);
