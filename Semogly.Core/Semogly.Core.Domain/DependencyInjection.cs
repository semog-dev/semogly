using Microsoft.Extensions.DependencyInjection;
using Semogly.Core.Domain.Shared.Abstractions;
using Semogly.Core.Domain.Shared.Common;

namespace Semogly.Core.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}