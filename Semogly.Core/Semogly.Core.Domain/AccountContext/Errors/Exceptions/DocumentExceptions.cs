using System;
using Semogly.Core.Domain.SharedContext.Exceptions;

namespace Semogly.Core.Domain.AccountContext.Errors.Exceptions;

public class DocumentNullException() : DomainException(ErrorMessages.Document.Null);
public class InvalidDocumentException() : DomainException(ErrorMessages.Document.Invalid);
