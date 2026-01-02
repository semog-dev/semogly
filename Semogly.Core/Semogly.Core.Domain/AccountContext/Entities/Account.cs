using Semogly.Core.Domain.AccountContext.Events;
using Semogly.Core.Domain.AccountContext.ValueObjects;
using Semogly.Core.Domain.SharedContext.Abstractions;
using Semogly.Core.Domain.SharedContext.Aggregates.Abstractions;
using Semogly.Core.Domain.SharedContext.Entities;
using Semogly.Core.Domain.SharedContext.ValueObjects;

namespace Semogly.Core.Domain.AccountContext.Entities;

public sealed class Account : Entity<int>, IAggregateRoot
{
    #region Constructors

    private Account() : base(Tracker.Create())
    {
    }

    private Account(
        CompositeName name,
        Email email,
        VerificationCode verificationCode,
        Password password,
        Tracker tracker,
        LockOut? lockout = null) : base(tracker)
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
        var tracker = Tracker.Create(dateTimeProvider);
        var account = new Account(name, email, verificationCode, password, tracker);
        account.RaiseDomainEvent(new OnAccountCreatedEvent(account.Id, name, email));

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
        RaiseDomainEvent(new OnResendEmailVerificationEvent(Id, Name, Email, VerificationCode));
    }

    public bool Authenticate(string plainTextPassword, IDateTimeProvider dateTimeProvider)
    {
        Tracker.Update(dateTimeProvider);
        return Password.Verify(Password.HashText, plainTextPassword);
    } 

    #endregion
}