using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Exceptions;

public class VerificationCodeLenghtException() : DomainException(ErrorMessages.VerificationCode.InvalidLength);
public class VerificationCodeNullException() : DomainException(ErrorMessages.VerificationCode.NullOrEmpty);
public class InvalidVerificationCodeException() : DomainException(ErrorMessages.VerificationCode.InvalidCode);
public class InactiveVerificationCodeException() : DomainException(ErrorMessages.VerificationCode.InvalidCode);