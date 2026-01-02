using Semogly.Core.Domain.AccountContext.Errors;
using Semogly.Core.Domain.AccountContext.Errors.Exceptions;
using Semogly.Core.Domain.SharedContext.ValueObjects;

namespace Semogly.Core.Domain.AccountContext.ValueObjects;

public sealed record LockOut : ValueObject
{
    #region Constants

    private const int MinLockOutReasonLength = 5;
    private const int MaxLockOutReasonLength = 50;

    #endregion
    
    #region Constructors

    private LockOut()
    {
    }

    private LockOut(DateTime? lockOutEndUtc, string? lockOutReason)
    {
        LockOutEndUtc = lockOutEndUtc;
        LockOutReason = lockOutReason;
    }

    #endregion

    #region Factories

    public static LockOut Create(int durationInMinutes, string? lockOutReason = null)
    {
        Validate(durationInMinutes, lockOutReason);
        return new LockOut(durationInMinutes == 0 ? null : DateTime.UtcNow.AddMinutes(durationInMinutes), lockOutReason);
    }

    #endregion

    #region Properties

    public DateTime? LockOutEndUtc { get; }
    public string? LockOutReason { get; }

    #endregion
    
    #region Methods

    private static void Validate(int durationInMinutes, string? lockOutReason)
    {
        if (durationInMinutes < 0)
            throw new InvalidLockOutExpiredException();

        if (lockOutReason is not null)
        {
            if (lockOutReason.Length is < MinLockOutReasonLength or > MaxLockOutReasonLength)
                throw new InvalidLockOutReasonLengthException();
        }
    }

    #endregion
}