using System;

namespace Semogly.Core.Domain.Shared.Abstractions;

public interface IIntegrationEventConvertible
{
    object ToIntegrationEvent();
}
