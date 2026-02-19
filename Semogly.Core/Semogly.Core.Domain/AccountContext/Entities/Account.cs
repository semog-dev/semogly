using Semogly.Core.Domain.AccountContext.Events;
using Semogly.Core.Domain.AccountContext.ValueObjects;
using Semogly.Core.Domain.Shared.Abstractions;
using Semogly.Core.Domain.Shared.SeedWork;

namespace Semogly.Core.Domain.AccountContext.Entities;

public sealed class Account : AuditableAggregateRoot<int>
{
    #region Constructors

    private Account() : base()
    {
    }

    private Account(
        CompositeName name,
        Email email,
        VerificationCode verificationCode,
        Password password,
        IDateTimeProvider dateTimeProvider,
        LockOut? lockout = null) : base(dateTimeProvider)
    {
        Name = name;
        Email = email;
        VerificationCode = verificationCode;
        Password = password;
        Lockout = lockout;
    }

    #endregion

    #region Factories

    public static Account CreateAccount(CompositeName name, Email email, Password password,
        IDateTimeProvider dateTimeProvider)
    {
        var verificationCode = VerificationCode.Create(dateTimeProvider);
        var account = new Account(name, email, verificationCode, password, dateTimeProvider);
        account.AddDomainEvent(new OnAccountCreatedEvent(account.PublicId, name, email, verificationCode, dateTimeProvider));

        return account;
    }

    #endregion

    #region Properties

    public CompositeName Name { get; } = null!;
    public Email Email { get; } = null!;
    public VerificationCode VerificationCode { get; private set; } = null!;
    public Password Password { get; } = null!;
    public LockOut? Lockout { get; } = LockOut.Create(0);

    #endregion

    #region Public Methods

    public void ResetVerificationCode(IDateTimeProvider dateTimeProvider)
    {
        VerificationCode = VerificationCode.Create(dateTimeProvider);
        AddDomainEvent(new OnResendEmailVerificationEvent(Id, Name, Email, VerificationCode, dateTimeProvider));
    }

    public bool Authenticate(string plainTextPassword, IDateTimeProvider dateTimeProvider)
    {
        SetUpdated(dateTimeProvider);
        return Password.Verify(Password.HashText, plainTextPassword);
    } 

    #endregion
}
