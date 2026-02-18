namespace Semogly.Core.Domain.Shared.Exceptions;

public class InvalidDateTimeProviderIsExpired(string message) : DomainException(message);