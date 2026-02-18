using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Semogly.Core.Domain.Shared;

namespace Semogly.Core.Api.SharedContext.Common;

public static class BuildExtension
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.Database.ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Postgres")
            ?? throw new Exception("ConnectionStrings__Postgres não encontrada");
        Configuration.Redis.Connection = Environment.GetEnvironmentVariable("ConnectionStrings__Redis")
            ?? throw new Exception("ConnectionStrings__Redis não encontrada");

        string keyVaultUri = builder.Configuration.GetValue<string>("KeyVaultURI")
            ?? throw new Exception("KeyVaultURI não encontrada");       

        builder.Configuration.AddAzureKeyVault(
            new Uri(keyVaultUri), 
            new DefaultAzureCredential());
        
        var jwtSection = builder.Configuration.GetSection("Jwt");

        Configuration.Jwt.Key = jwtSection.GetValue<string>("Key") ?? string.Empty;
        Configuration.Jwt.Issuer = jwtSection.GetValue<string>("Issuer") ?? string.Empty;
        Configuration.Jwt.Audience = jwtSection.GetValue<string>("Audience") ?? string.Empty;
        Configuration.Jwt.AccessTokenExpirationMinutes = jwtSection.GetValue<int>("AccessTokenExpirationMinutes");

        var secretsSection = builder.Configuration.GetSection("Secrets");

        Configuration.Security.PasswordSaltKey = secretsSection.GetValue<string>("PasswordSaltKey") ?? string.Empty;
        Configuration.Security.SaltSize = secretsSection.GetValue<short>("SaltSize");
        Configuration.Security.KeySize = secretsSection.GetValue<short>("KeySize");
        Configuration.Security.Iterations = secretsSection.GetValue<short>("Iterations");
        Configuration.Security.SplitChar = secretsSection.GetValue<char>("SplitChar");
        Configuration.Security.RefreshTokenSecret = secretsSection.GetValue<string>("RefreshTokenSecret") ?? string.Empty;

        var redisSection = builder.Configuration.GetSection("Redis");

        Configuration.Redis.RefreshTokenPrefix = redisSection.GetValue<string>("RefreshTokenPrefix") ?? string.Empty;

        var mediatRSection =  builder.Configuration.GetSection("MediatR");

        Configuration.MediatR.LicenseKey = mediatRSection.GetValue<string>("LicenseKey") ?? string.Empty;

        return builder;
    }
}