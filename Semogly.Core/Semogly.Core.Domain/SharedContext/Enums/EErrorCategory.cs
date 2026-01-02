namespace Semogly.Core.Domain.SharedContext.Enums;

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
