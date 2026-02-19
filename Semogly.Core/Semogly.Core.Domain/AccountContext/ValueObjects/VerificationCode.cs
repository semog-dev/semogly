using Semogly.Core.Domain.AccountContext.Exceptions;
using Semogly.Core.Domain.Shared.Abstractions;
using Semogly.Core.Domain.Shared.SeedWork;

namespace Semogly.Core.Domain.AccountContext.ValueObjects;

public sealed record VerificationCode : ValueObject
{
    #region Constants

    private const int MinLength = 6;

    #endregion

    #region Constructors

    private VerificationCode()
    {
    }

    private VerificationCode(string code, DateTime expiresAtUtc)
    {
        Code = code;
        ExpiresAtUtc = expiresAtUtc;
    }

    #endregion

    #region Factories

    public static VerificationCode Create(IDateTimeProvider dateTimeProvider)
    {
        return new VerificationCode(
            Guid.NewGuid().ToString("N")[..MinLength].ToUpper(),
            dateTimeProvider.UtcNow.AddMinutes(5));
    }

    #endregion

    #region Methods

    public void Verify(string code, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrEmpty(code))
            throw new VerificationCodeNullException();

        if (string.IsNullOrWhiteSpace(code))
            throw new VerificationCodeNullException();

        if (code.Length != MinLength)
            throw new VerificationCodeLenghtException();

        if (Code != code)
            throw new InvalidVerificationCodeException();

        if (IsActive)
            throw new InactiveVerificationCodeException();

        if (IsExpired)
            throw new InactiveVerificationCodeException();

        VerifiedAtUtc = dateTimeProvider.UtcNow;
        ExpiresAtUtc = null;
    }

    #endregion

    #region Operators

    public static implicit operator string(VerificationCode verificationCode)
    {
        return verificationCode.ToString();
    }

    #endregion

    #region Others

    public override string ToString()
    {
        return Code;
    }

    #endregion

    #region Properties

    public string Code { get; } = string.Empty;
    public DateTime? ExpiresAtUtc { get; private set; }
    public DateTime? VerifiedAtUtc { get; private set; }
    public bool IsActive => !IsExpired && VerifiedAtUtc.HasValue;
    public bool IsExpired => ExpiresAtUtc is not null && ExpiresAtUtc.Value <= DateTime.UtcNow;

    #endregion
}