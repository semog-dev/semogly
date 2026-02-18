using Azure.Identity;
using Semogly.Core.Domain.Shared;

namespace Semogly.Core.Api.SharedContext.Common;

public static class BuildExtension
{
    public static HostApplicationBuilder AddConfiguration(this HostApplicationBuilder builder)
    {
        Configuration.Database.ConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Postgres")
            ?? throw new Exception("ConnectionStrings__Postgres não encontrada");

        string keyVaultUri = builder.Configuration.GetValue<string>("KeyVaultURI")
            ?? throw new Exception("KeyVaultURI não encontrada");       

        builder.Configuration.AddAzureKeyVault(
            new Uri(keyVaultUri), 
            new DefaultAzureCredential());

        var mailgunSection = builder.Configuration.GetSection("Mailgun");    

        Configuration.Mailgun.ApiKey = mailgunSection.GetValue<string>("ApiKey") ?? string.Empty;
        Configuration.Mailgun.BaseUrl = mailgunSection.GetValue<string>("BaseURL") ?? string.Empty;
        Configuration.Mailgun.Domain = mailgunSection.GetValue<string>("Domain") ?? string.Empty;
        Configuration.Mailgun.From = mailgunSection.GetValue<string>("From") ?? string.Empty;
        return builder;
    }
}