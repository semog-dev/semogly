using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Exceptions;

public class InvalidCompositeNameLengthException() : DomainException(ErrorMessages.CompositeName.InvalidLength);
public class InvalidLastNameException() : DomainException(ErrorMessages.Name.Invalid);
public class InvalidMiddleNameException() : DomainException(ErrorMessages.Name.Invalid);
public class InvalidNameException() : DomainException(ErrorMessages.Name.Invalid);
public class InvalidNameLengthException() : DomainException(ErrorMessages.Name.InvalidLength);
public class NameNullOrEmptyException() : DomainException(ErrorMessages.Name.InvalidNullOrEmpty);
