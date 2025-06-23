using SaltStackers.Application.Interfaces;
using SaltStackers.Common.Helper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;

namespace SaltStackers.Application.Services.EmailHandler;

public class GoogleEmailSender : IEmailSender
{
    private const string ApplicationName = "SaltStackers";
    private const string From = "admin@saltstackers.com";

    public async Task SendEmailAsync(string[] to, string subject, string body, bool isBodyHtml = true, List<IFormFile>? attachments = null)
    {
        try
        {
            string[] Scopes = { GmailService.Scope.GmailSend, GmailService.Scope.GmailReadonly,
                GmailService.Scope.GmailCompose, GmailService.Scope.GmailModify,
                GmailService.Scope.MailGoogleCom };

            var credentialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Credentials", "gmail-api.p12");
            var serviceAccountEmail = "gmail-service@saltstackers.iam.gserviceaccount.com";

            var certificate = X509CertificateLoader.LoadPkcs12FromFile(credentialPath, "notasecret", X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.Exportable);

            var credential = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer(serviceAccountEmail)
               {
                   User = From,
                   Scopes = Scopes
               }.FromCertificate(certificate));

            if (await credential.RequestAccessTokenAsync(CancellationToken.None))
            {
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                var mail = new MailMessage
                {
                    Subject = subject,
                    Body = body,
                    From = new MailAddress(From, ApplicationName),
                    IsBodyHtml = isBodyHtml
                };

                if (attachments != null && attachments.Any())
                {
                    foreach (var attachment in attachments)
                    {
                        if (attachment != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                attachment.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                var att = new Attachment(new MemoryStream(fileBytes), attachment.FileName);
                                mail.Attachments.Add(att);
                            }
                        }
                    }
                }

                foreach (var item in to)
                {
                    mail.To.Add(new MailAddress(item));
                }

                var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mail);

                var message = new Message
                {
                    Raw = UrlHelper.Base64UrlEncode(mimeMessage.ToString())
                };
                var response = service.Users.Messages.Send(message, "me").Execute();
            }
        }
        catch
        {
            //Ignored
        }
    }
}
