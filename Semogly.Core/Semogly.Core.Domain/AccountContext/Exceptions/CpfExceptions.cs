using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Exceptions;

public class CpfNullOrEmptyException() : DomainException(ErrorMessages.Cpf.NullOrEmpty);
public class InvalidCpfException() : DomainException(ErrorMessages.Cpf.Invalid);
public class InvalidCpfFormatException() : DomainException(ErrorMessages.Cpf.Invalid);
public class InvalidCpfLenghtException() : DomainException(ErrorMessages.Cpf.InvalidLength);
public class InvalidCpfNumberException() : DomainException(ErrorMessages.Cpf.InvalidNumber);