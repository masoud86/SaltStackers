using AutoMapper;
using SaltStackers.Application.Helpers;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.Services.EmailHandler;
using SaltStackers.Application.ViewModels.Financial;
using SaltStackers.Application.ViewModels.Message;
using SaltStackers.Common.Helper;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Membership;
using SaltStackers.Domain.Models.Message;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace SaltStackers.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _iMapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IOperationService _operationService;
        private readonly IFinancialRepository _financialRepository;

        public EmailService(IMessageRepository messageRepository, IConfiguration configuration,
            UserManager<AspNetUser> userManager, IOperationService operationService,
            IFinancialRepository financialRepository)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmailGateway, EmailGatewayDto>();
            });
            _iMapper = config.CreateMapper();
            _messageRepository = messageRepository;
            _configuration = configuration;
            _userManager = userManager;
            _operationService = operationService;
            _financialRepository = financialRepository;
        }

        public EmailGateway GetDefaultEmailGateway()
        {
            return _messageRepository.GetDefaultEmailGateway();
        }

        public async Task SendEmailByDefaultGatewayAsync(string[] to, string subject, string body, bool isBodyHtml = true)
        {
            if (_configuration.GetSection("Email:Enabled").Get<bool>())
            {
                var gateway = _iMapper.Map<EmailGatewayDto>(GetDefaultEmailGateway());
                IEmailSender sender = new GatewayEmailSender(gateway);
                await sender.SendEmailAsync(to, subject, body, isBodyHtml);
            }
        }

        public async Task SendEmailByGmailApiAsync(string[] to, string subject, string body, bool isBodyHtml = true)
        {
            if (_configuration.GetSection("Email:Enabled").Get<bool>())
            {
                IEmailSender sender = new GoogleEmailSender();
                await sender.SendEmailAsync(to, subject, body, isBodyHtml);
            }
        }

        public async Task SendEmailByGmailApiAsync(string[] to, string subject, string body, bool isBodyHtml = true, List<IFormFile>? attachments = null)
        {
            if (_configuration.GetSection("Email:Enabled").Get<bool>())
            {
                IEmailSender sender = new GoogleEmailSender();
                await sender.SendEmailAsync(to, subject, body, isBodyHtml, attachments);
            }
        }

        public async Task Register(string userId)
        {
            if (_configuration.GetSection("Email:WelcomeEmail").Get<bool>())
            {
                var user = await _userManager.FindByIdAsync(userId);
                var discountBody = "";
                
                var body = TemplateHelper.GenerateEmailBody("Email/Membership/Welcome", new
                {
                    Customer = user.Name,
                    WelcomeDiscount = discountBody
                }, _configuration.GetSection("DevelopmentMode").Get<bool>());
                
                await SendEmailByGmailApiAsync(new[] { user.Email.ToLower().Trim() },
                        "Welcome to SaltStackers", body, true);
            }
        }
    }
}
