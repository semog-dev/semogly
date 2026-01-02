using Semogly.Core.Domain.SharedContext;

namespace Semogly.Core.Api.SharedContext.Common;

public static class BuildExtension
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.Database.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("ConnectionString não encontrada");

        var jwtSection = builder.Configuration.GetSection("Jwt");

        Configuration.Jwt.Issuer = jwtSection.GetValue<string>("Issuer") ?? string.Empty;
        Configuration.Jwt.Audience = jwtSection.GetValue<string>("Audience") ?? string.Empty;
        Configuration.Jwt.Key = jwtSection.GetValue<string>("Key") ?? string.Empty;
        Configuration.Jwt.AccessTokenExpirationMinutes = jwtSection.GetValue<int>("AccessTokenExpirationMinutes");

        var secretsSection = builder.Configuration.GetSection("Secrets");

        Configuration.Security.PasswordSaltKey = secretsSection.GetValue<string>("PasswordSaltKey") ?? string.Empty;
        Configuration.Security.SaltSize = secretsSection.GetValue<short>("SaltSize");
        Configuration.Security.KeySize = secretsSection.GetValue<short>("KeySize");
        Configuration.Security.Iterations = secretsSection.GetValue<short>("Iterations");
        Configuration.Security.SplitChar = secretsSection.GetValue<char>("SplitChar");
        Configuration.Security.RefreshTokenSecret = secretsSection.GetValue<string>("RefreshTokenSecret") ?? string.Empty;

        var redisSection = builder.Configuration.GetSection("Redis");

        Configuration.Redis.Connection = redisSection.GetValue<string>("Connection") ?? string.Empty;
        Configuration.Redis.RefreshTokenPrefix = redisSection.GetValue<string>("RefreshTokenPrefix") ?? string.Empty;

        var mediatRSection =  builder.Configuration.GetSection("MediatR");

        Configuration.MediatR.LicenseKey = mediatRSection.GetValue<string>("LicenseKey") ?? string.Empty;

        return builder;
    }
}