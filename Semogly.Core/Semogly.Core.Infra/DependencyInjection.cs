using Microsoft.Extensions.DependencyInjection;
using Semogly.Core.Domain.AccountContext.Services.Abstractions;
using Semogly.Core.Domain.SharedContext;
using Semogly.Core.Infra.AccountContext.Services;
using StackExchange.Redis;

namespace Semogly.Core.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = Configuration.Redis.Connection;
            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IRefreshTokenService, RefreshTokenService>();

        return services;
    }
}
