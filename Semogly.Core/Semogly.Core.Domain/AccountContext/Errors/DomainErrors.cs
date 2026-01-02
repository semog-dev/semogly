using Semogly.Core.Domain.SharedContext.Enums;
using Semogly.Core.Domain.SharedContext.Results;

namespace Semogly.Core.Domain.AccountContext.Errors;

public static class DomainErrors
{
    public static AccountError Account { get; } = new();

    public sealed class AccountError
    {
        public readonly Error NotFound = new("ACCOUNT.NOT_FOUND", ErrorMessages.Account.NotFound, EErrorCategory.Conflict);
        public readonly Error IsInactive = new("ACCOUNT.INACTIVE", ErrorMessages.Account.IsInactive, EErrorCategory.Conflict);
        public readonly Error IsAlreadyActive = new("ACCOUNT.ALREADY_ACTIVE", ErrorMessages.Account.IsAlreadyActive, EErrorCategory.Conflict);
        public readonly Error LockedOut = new("ACCOUNT.LOCKED_OUT", ErrorMessages.Account.IsLockedOut, EErrorCategory.Conflict);
        public readonly Error EmailInUse = new("ACCOUNT.EMAIL.CONFLICT", ErrorMessages.Account.EmailInUse, EErrorCategory.Conflict);
        public readonly Error PasswordIsInvalid = new("ACCOUNT.PASSWORD.INVALID", ErrorMessages.Account.PasswordIsInvalid, EErrorCategory.Conflict);
        public readonly Error EmailIsDifferent = new("ACCOUNT.EMAIL.DIFFERENT", ErrorMessages.Account.EmailIsDifferent, EErrorCategory.Conflict);
        public readonly Error DocumentIsDifferent = new("ACCOUNT.DOCUMENT.DIFFERENT", ErrorMessages.Account.DocumentIsDifferent, EErrorCategory.Conflict);
    }
}