using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semogly.Core.Domain.SharedContext.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}