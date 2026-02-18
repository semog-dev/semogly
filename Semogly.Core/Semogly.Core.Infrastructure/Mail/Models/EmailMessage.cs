using System;

namespace Semogly.Core.Infrastructure.Mail.Models;

public class EmailMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Remetente { get; set; } = string.Empty;
    public string Assunto { get; set; } = string.Empty;
    public string Corpo { get; set; } = string.Empty;
    public DateTime Data { get; set; } = DateTime.Now;
    public bool Lido { get; set; } = false;
}
