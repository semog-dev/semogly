using System;
using RestSharp;
using RestSharp.Authenticators;
using Semogly.Core.Domain.Shared;
using Semogly.Core.Infrastructure.Mail.Interfaces;
using Semogly.Core.Infrastructure.Mail.Models;

namespace Semogly.Core.Infrastructure.Mail.Services;

public class MailgunService : IEmailService
{
    public async Task Send(EmailMessage emailMessage)
    {
        var options = new RestClientOptions(Configuration.Mailgun.BaseUrl)
        {
            Authenticator = new HttpBasicAuthenticator("api", Configuration.Mailgun.ApiKey)
        };

        var client = new RestClient(options);
        var request = new RestRequest($"/v3/{Configuration.Mailgun.Domain}/messages", Method.Post)
        {
            AlwaysMultipartFormData = true,
        };
        request.AddParameter("from", emailMessage.From);

        foreach (string to in emailMessage.To)
        {
            request.AddParameter("to", to);
        }

        foreach (string toCc in emailMessage.ToCc)
        {
            request.AddParameter("cc", toCc);
        }

        foreach (string toCco in emailMessage.ToCco)
        {
            request.AddParameter("bcc", toCco);
        }

        request.AddParameter("subject", emailMessage.Subject);
        _ = emailMessage.TemplateName is not null
            ? request.AddParameter("template", "account-confirmation")
            : emailMessage.IsHtml 
                ? request.AddParameter("html", emailMessage.Body)
                : request.AddParameter("text", emailMessage.Body);

        if (emailMessage.Variables is not null)
        {
            foreach (var (chave, valor) in emailMessage.Variables)
            {
                request.AddParameter("h:X-Mailgun-Variables", $"{{\"{chave}\": \"{valor}\"}}");
            }
        }

        await client.ExecuteAsync(request);
    }
}
