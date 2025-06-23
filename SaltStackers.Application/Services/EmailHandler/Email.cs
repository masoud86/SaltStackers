using SaltStackers.Application.Interfaces;

namespace SaltStackers.Application.Services.EmailHandler;

public abstract class Email
{
    public IEmailSender MessageSender { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public bool IsBodyHtml { get; }

    public string[] To { get; }

    public abstract Task SendAsync();
}
