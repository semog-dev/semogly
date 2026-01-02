using System;
using Semogly.Core.Domain.SharedContext.Enums;
using Semogly.Core.Domain.SharedContext.Results;

namespace Semogly.Core.Api.SharedContext.Common;

public static class ErrorHttpMapper
{
    public static int ToStatusCode(this Error error) =>
        error.Category switch
        {
            EErrorCategory.Validation   => 400,
            EErrorCategory.Conflict     => 409,
            EErrorCategory.NotFound     => 404,
            EErrorCategory.BusinessRule => 422,
            EErrorCategory.Unauthorized => 401,
            EErrorCategory.Forbidden    => 403,
            _ => 500
        };
}
