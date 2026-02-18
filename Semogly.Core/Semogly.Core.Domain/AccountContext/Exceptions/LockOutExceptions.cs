using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Exceptions;

public class InvalidLockOutExpiredException() : DomainException(ErrorMessages.LockOut.InvalidLockOutExpired);
public class InvalidLockOutReasonLengthException() : DomainException(ErrorMessages.LockOut.InvalidLockOutReasonLength);
