namespace Semogly.Core.Domain.SharedContext.Exceptions;

public class InvalidDateTimeProviderIsExpired(string message) : DomainException(message);