using System;
using Semogly.Core.Infrastructure.Mail.Models;

namespace Semogly.Core.Infrastructure.Mail.Interfaces;

public interface IEmailService
{
    Task Send(EmailMessage emailMessage);
}
