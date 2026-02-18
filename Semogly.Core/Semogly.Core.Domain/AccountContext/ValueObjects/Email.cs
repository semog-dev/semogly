using System.Text.RegularExpressions;
using Semogly.Core.Domain.AccountContext.Exceptions;
using Semogly.Core.Domain.Shared.Common;
using Semogly.Core.Domain.Shared.SeedWork;

namespace Semogly.Core.Domain.AccountContext.ValueObjects;

public partial record Email : ValueObject
{
    #region Constants

    private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

    #endregion

    #region Constructors

    private Email()
    {
    }

    private Email(string address, string hash)
    {
        Address = address;
        Hash = hash;
    }

    #endregion

    #region Factories

    public static Email Create(string address)
    {
        if (string.IsNullOrEmpty(address) || string.IsNullOrWhiteSpace(address))
            throw new EmailNullOrEmptyException();
        
        address = address.Trim();
        address = address.ToLower();
        
        if (!EmailRegex().IsMatch(address))
            throw new InvalidEmailException();

        return new Email(address, address.ToBase64());
    }

    #endregion

    #region Operators

    public static implicit operator string(Email email)
    {
        return email.ToString();
    }

    #endregion

    #region Overrides

    public override string ToString()
    {
        return Address;
    }

    #endregion

    #region Other

    [GeneratedRegex(Pattern)]
    private static partial Regex EmailRegex();

    #endregion

    #region Properties

    public string Address { get; } = string.Empty;
    public string Hash { get; } = string.Empty;

    #endregion
}