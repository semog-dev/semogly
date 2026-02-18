namespace Semogly.Core.Domain.Shared.Primitives;

public enum EErrorCategory
{
    None,
    Validation,
    Conflict,
    NotFound,
    BusinessRule,
    Unauthorized,
    Forbidden,
    Invariant
}
