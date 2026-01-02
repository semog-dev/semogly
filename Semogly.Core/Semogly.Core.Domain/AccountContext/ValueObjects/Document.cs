using Semogly.Core.Domain.AccountContext.Enums;
using Semogly.Core.Domain.AccountContext.Errors;
using Semogly.Core.Domain.AccountContext.Errors.Exceptions;

namespace Semogly.Core.Domain.AccountContext.ValueObjects;

public record Document
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