using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Message;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;

namespace SaltStackers.Application.Services.EmailHandler;

public class GatewayEmailSender : IEmailSender
{
    private EmailGatewayDto Gateway { get; }

    public GatewayEmailSender(EmailGatewayDto gateway)
    {
        Gateway = gateway;
    }

    public async Task SendEmailAsync(string[] to, string subject, string body, bool isBodyHtml = true, List<IFormFile>? attachment = null)
    {
        try
        {
            using var message = new MailMessage
            {
                From = new MailAddress(Gateway.From, Gateway.Display),
                Subject = subject,
                IsBodyHtml = isBodyHtml,
                Body = body
            };

            foreach (var item in to)
            {
                message.To.Add(new MailAddress(item));
            }

            using var smtpClient = new SmtpClient
            {
                Port = Gateway.Port,
                Host = Gateway.Host,
                EnableSsl = Gateway.EnableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Gateway.Username, Gateway.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            await smtpClient.SendMailAsync(message);
        }
        catch
        {
            // ignored
        }
    }
}
