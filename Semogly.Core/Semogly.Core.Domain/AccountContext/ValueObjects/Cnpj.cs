using Semogly.Core.Domain.AccountContext.Enums;
using Semogly.Core.Domain.AccountContext.Errors;
using Semogly.Core.Domain.AccountContext.Errors.Exceptions;
using Semogly.Core.Domain.SharedContext.Common;

namespace Semogly.Core.Domain.AccountContext.ValueObjects;

public sealed record Cnpj : Document
{
    #region Constants

    private const short MinLength = 14;

    #endregion

    #region Constructors

    private Cnpj(string number) : base(number, EDocumentType.Cnpj)
    {
    }

    #endregion

    #region Factories

    public static Cnpj Create(string number)
    {
        number = number.ToNumbers();
        Validate(number);
        return new Cnpj(number);
    }

    #endregion

    #region Overrides

    public override string ToString()
    {
        return Number;
    }

    #endregion

    #region Methods

    private static void Validate(string number)
    {
        if (string.IsNullOrEmpty(number) || string.IsNullOrWhiteSpace(number))
            throw new CnpjNullOrEmptyException();
        
        if (number.Length != MinLength)
            throw new InvalidCnpjLenghtException();

        if (number.All(c => c == number[0]))
            throw new InvalidCnpjNumberException();

        var multiplier1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplier2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var temp = number[..12];
        var sum = 0;

        for (var i = 0; i < 12; i++)
            sum += int.Parse(temp[i].ToString()) * multiplier1[i];

        var rest = sum % 11;
        rest = rest < 2 ? 0 : 11 - rest;
        var digit = rest.ToString();

        temp += digit;
        sum = 0;

        for (var i = 0; i < 13; i++)
            sum += int.Parse(temp[i].ToString()) * multiplier2[i];

        rest = sum % 11;
        rest = rest < 2 ? 0 : 11 - rest;
        digit += rest.ToString();

        if (!number.EndsWith(digit))
            throw new InvalidCnpjException();
    }

    #endregion

    #region Operators

    public static implicit operator Cnpj(string number)
    {
        return Create(number);
    }

    public static implicit operator string(Cnpj cnpj)
    {
        return cnpj.Number;
    }

    #endregion
}