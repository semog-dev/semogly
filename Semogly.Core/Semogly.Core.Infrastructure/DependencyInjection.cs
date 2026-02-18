using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Semogly.Core.Application.AuthContext.Abstractions;
using Semogly.Core.Application.SharedContext.Abstractions.Persistence;
using Semogly.Core.Domain.Shared;
using Semogly.Core.Infrastructure.Authentication;
using Semogly.Core.Infrastructure.Persistence;
using Semogly.Core.Infrastructure.Persistence.Redis;
using Semogly.Core.Infrastructure.Persistence.Repositories;
using Semogly.Core.Infrastructure.Persistence.UOW;
using StackExchange.Redis;

namespace Semogly.Core.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = Configuration.Redis.Connection;
            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ISessionStore, SessionStore>();

        return services;
    }

    public static IServiceCollection AddData(this IServiceCollection services)
    {
        services.AddDbContext<SemoglyDbContext>(options =>
        {
           options.UseNpgsql(Configuration.Database.ConnectionString); 
        });

        return services;
    }

    public static IServiceCollection AddUnitOfWorks(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IAccountRepository, AccountRepository>();

        return services;
    }
}
