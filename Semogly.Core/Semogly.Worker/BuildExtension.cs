using Semogly.Core.Domain.Shared;

namespace Semogly.Core.Api.SharedContext.Common;

public static class BuildExtension
{
    public static HostApplicationBuilder AddConfiguration(this HostApplicationBuilder builder)
    {
        Configuration.Database.ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Postgres")
            ?? throw new Exception("ConnectionStrings__Postgres não encontrada");

        return builder;
    }
}