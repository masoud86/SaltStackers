namespace SaltStackers.Application.Services.EmailHandler;

public class GoogleEmail : Email
{
    public override async Task SendAsync()
    {
        await MessageSender.SendEmailAsync(To, Subject, Body, IsBodyHtml);
    }
}
