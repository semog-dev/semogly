using Semogly.Core.Domain.Shared.Errors;
using Semogly.Core.Domain.Shared.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Exceptions;

public class DocumentNullException() : DomainException(ErrorMessages.Document.Null);
public class InvalidDocumentException() : DomainException(ErrorMessages.Document.Invalid);
