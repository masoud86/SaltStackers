using Microsoft.AspNetCore.Http;
using SaltStackers.Domain.Models.Message;

namespace SaltStackers.Application.Interfaces
{
    public interface IEmailService
    {
        EmailGateway GetDefaultEmailGateway();

        Task SendEmailByDefaultGatewayAsync(string[] to, string subject, string body, bool isBodyHtml = true);

        Task SendEmailByGmailApiAsync(string[] to, string subject, string body, bool isBodyHtml = true, List<IFormFile>? attachments = null);

        Task Register(string userId);
    }
}
