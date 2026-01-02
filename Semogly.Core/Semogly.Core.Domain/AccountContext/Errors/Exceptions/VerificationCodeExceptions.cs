using System;
using Semogly.Core.Domain.SharedContext.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Errors.Exceptions;

public class VerificationCodeLenghtException() : DomainException(ErrorMessages.VerificationCode.InvalidLength);
public class VerificationCodeNullException() : DomainException(ErrorMessages.VerificationCode.NullOrEmpty);
public class InvalidVerificationCodeException() : DomainException(ErrorMessages.VerificationCode.InvalidCode);
public class InactiveVerificationCodeException() : DomainException(ErrorMessages.VerificationCode.InvalidCode);