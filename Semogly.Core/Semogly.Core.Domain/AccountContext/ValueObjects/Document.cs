using Semogly.Core.Domain.AccountContext.Enums;
using Semogly.Core.Domain.AccountContext.Exceptions;
using Semogly.Core.Domain.Shared.SeedWork;

namespace Semogly.Core.Domain.AccountContext.ValueObjects;

public record Document : ValueObject
{
    #region Constructors

    private Document()
    {
    }

    protected Document(string number, EDocumentType type)
    {
        if (string.IsNullOrEmpty(number))
            throw new DocumentNullException();

        if (string.IsNullOrWhiteSpace(number))
            throw new InvalidDocumentException();

        Number = number.Trim();
        Type = type;
    }

    #endregion

    #region Properties

    public string Number { get; } = string.Empty;
    public EDocumentType Type { get; }

    #endregion

    #region Operators

    public static implicit operator string(Document document) => document.Number;

    #endregion
}