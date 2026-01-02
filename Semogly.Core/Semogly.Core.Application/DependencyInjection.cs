using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Semogly.Core.Application.SharedContext.Behaviors;
using Semogly.Core.Domain.SharedContext;

namespace Semogly.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.LicenseKey = Configuration.MediatR.LicenseKey;
        });

        return services;
    }

    public static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}