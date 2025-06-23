namespace SaltStackers.Application.Services.EmailHandler;

public class GatewayEmail : Email
{
    public override async Task SendAsync()
    {
        await MessageSender.SendEmailAsync(To, Subject, Body, IsBodyHtml);
    }
}
