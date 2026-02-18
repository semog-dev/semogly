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
            AlwaysMultipartFormData = true
        };
        request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox7dd6d0e6a3c84904a7ec6e9179beda4c.mailgun.org>");
        request.AddParameter("to", "Fernando Gomes Pereira <semogdev.pereira@hotmail.com>");
        request.AddParameter("subject", "Hello Fernando Gomes Pereira");
        request.AddParameter("text", "Congratulations Fernando Gomes Pereira, you just sent an email with Mailgun! You are truly awesome!");
        await client.ExecuteAsync(request);
    }
}
