namespace Semogly.Core.Domain.SharedContext.Results;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Um valor nulo foi fornecido.");
    public static Error Validation(string code, string message, object? metadata = null)
        => new(code, message) { Metadata = metadata };

    public object? Metadata { get; init; }
}