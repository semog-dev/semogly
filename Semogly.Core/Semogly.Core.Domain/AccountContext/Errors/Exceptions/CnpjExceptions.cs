using System;
using Semogly.Core.Domain.SharedContext.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Errors.Exceptions;

public class CnpjNullOrEmptyException() : DomainException(ErrorMessages.Cnpj.NullOrEmpty);
public class InvalidCnpjException() : DomainException(ErrorMessages.Cnpj.Invalid);
public class InvalidCnpjLenghtException() : DomainException(ErrorMessages.Cnpj.InvalidLength);
public class InvalidCnpjNumberException() : DomainException(ErrorMessages.Cnpj.InvalidNumber);
