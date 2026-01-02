using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Semogly.Core.Domain.AccountContext.Repositories.Abstractions;
using Semogly.Core.Domain.AccountContext.Services.Abstractions;
using Semogly.Core.Domain.SharedContext;
using Semogly.Core.Domain.SharedContext.Data.Abstractions;
using Semogly.Core.Infrastructure.Data;
using Semogly.Core.Infrastructure.Data.Repositories;
using Semogly.Core.Infrastructure.Data.UOW;
using Semogly.Core.Infrastructure.Services;
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
        services.AddTransient<ISessionService, SessionService>();

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
