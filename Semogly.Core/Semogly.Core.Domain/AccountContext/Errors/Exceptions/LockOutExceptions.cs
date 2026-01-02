using System;
using Semogly.Core.Domain.SharedContext.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Errors.Exceptions;

public class InvalidLockOutExpiredException() : DomainException(ErrorMessages.LockOut.InvalidLockOutExpired);
public class InvalidLockOutReasonLengthException() : DomainException(ErrorMessages.LockOut.InvalidLockOutReasonLength);
