namespace Semogly.Core.Domain.AccountContext.Events;

public sealed class AccountCreatedIntegrationEvent(Guid accountPublicId, string name, string email, string verificationCode)
{
    public Guid AccountPublicId { get; } = accountPublicId;
    public string Name { get; } = name;
    public string Email { get; } = email;
    public string VerificationCode { get; } = verificationCode;
}

