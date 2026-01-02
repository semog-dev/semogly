using Semogly.Core.Domain.SharedContext.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Errors.Exceptions;

public class CpfNullOrEmptyException() : DomainException(ErrorMessages.Cpf.NullOrEmpty);
public class InvalidCpfException() : DomainException(ErrorMessages.Cpf.Invalid);
public class InvalidCpfFormatException() : DomainException(ErrorMessages.Cpf.Invalid);
public class InvalidCpfLenghtException() : DomainException(ErrorMessages.Cpf.InvalidLength);
public class InvalidCpfNumberException() : DomainException(ErrorMessages.Cpf.InvalidNumber);