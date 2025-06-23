using Microsoft.AspNetCore.Http;

namespace SaltStackers.Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string[] to, string subject, string body, bool isBodyHtml = true, List<IFormFile>? attachments = null);
    }
}
