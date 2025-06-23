using SaltStackers.Application.Custom;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.Services;
using SaltStackers.Data.Repository;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SaltStackers.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection service)
        {
            service.AddScoped<IUserClaimsPrincipalFactory<AspNetUser>, CustomUserClaimsPrincipalFactory>();

            //Aplication Layer
            service.AddTransient(typeof(ILoggerService<>), typeof(LoggerService<>));
            service.AddScoped<ILogService, LogService>();
            service.AddScoped<IEmailService, EmailService>();
            service.AddScoped<IMembershipService, MembershipService>();
            service.AddScoped<IWebRequestService, WebRequestService>();
            service.AddScoped<IApplicationService, ApplicationService>();
            service.AddScoped<IFinancialService, FinancialService>();
            service.AddScoped<IOtpService, OtpService>();
            service.AddScoped<ICacheService, CacheService>();
            service.AddScoped<ICustomerService, CustomerService>();
            service.AddScoped<INutritionService, NutritionService>();
            service.AddScoped<IOperationService, OperationService>();
            service.AddScoped<IUploadService, UploadService>();
            service.AddScoped<ITokenService, TokenService>();

            //Infrastructure Data Layer
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddScoped<ILogRepository, LogRepository>();
            service.AddScoped<IMessageRepository, MessageRepository>();
            service.AddScoped<IApplicationRepository, ApplicationRepository>();
            service.AddScoped<IFinancialRepository, FinancialRepository>();
            service.AddScoped<ICustomerRepository, CustomerRepository>();
            service.AddScoped<INutritionRepository, NutritionRepository>();
            service.AddScoped<IOperationRepository, OperationRepository>();
        }
    }
}
