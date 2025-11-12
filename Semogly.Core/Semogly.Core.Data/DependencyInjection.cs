using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Semogly.Core.Data.AccountContext;
using Semogly.Core.Data.AccountContext.Repositories;
using Semogly.Core.Domain.AccountContext.Repositories.Abstractions;
using Semogly.Core.Domain.SharedContext;
using Semogly.Core.Domain.SharedContext.Data.Abstractions;
using Semogly.Core.Domain.SharedContext.Repositories.Abstractions;

namespace Semogly.Core.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddDbContext<AccountDbContext>(options =>
        {
           options.UseNpgsql(Configuration.Database.ConnectionString);
           options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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