using System;

namespace Semogly.Core.Infrastructure.Mail.Models;

public class EmailMessage
{

    public EmailMessage(
        string subject, 
        string body, 
        string from,
        bool isHtml = false, 
        IDictionary<string, object>? variables = null,
        IEnumerable<string>? to = null,
        IEnumerable<string>? toCc = null,
        IEnumerable<string>? toCco = null
    )
    {
        Subject = subject;
        Body = body;
        IsHtml = isHtml;
        From = from;
        Variables = variables;
        To = to ?? [];
        ToCc = toCc ?? [];
        ToCco = toCco ?? [];
    }
    public EmailMessage(
        string subject,
        string from, 
        string templateName,
        IDictionary<string, object>? variables = null,
        IEnumerable<string>? to = null,
        IEnumerable<string>? toCc = null,
        IEnumerable<string>? toCco = null)
    {
        Subject = subject;
        TemplateName = templateName;
        From = from;
        Variables = variables;
        To = to ?? [];
        ToCc = toCc ?? [];
        ToCco = toCco ?? [];
    }


    public string Subject { get; set; }
    public string? Body { get; set; }
    public bool IsHtml { get; set; }
    public string From { get; set; }
    public string? TemplateName { get; set; }
    public IDictionary<string, object>? Variables { get; set; }
    public IEnumerable<string> To { get; set; }
    public IEnumerable<string> ToCc { get; set; }
    public IEnumerable<string> ToCco { get; set; }
}
