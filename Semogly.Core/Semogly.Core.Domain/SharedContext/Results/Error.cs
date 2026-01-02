using Semogly.Core.Domain.SharedContext.Enums;

namespace Semogly.Core.Domain.SharedContext.Results;

public record Error(string Code, string Message, EErrorCategory Category, object? Metadata = null)
{
    public static readonly Error None = new(string.Empty, string.Empty, EErrorCategory.None);
    public static readonly Error NullValue = new("Error.NullValue", "Um valor nulo foi fornecido.", EErrorCategory.NotFound);
    public static Error Validation(string code, string message, object? metadata = null)
        => new(code, message, EErrorCategory.Validation, metadata);
}