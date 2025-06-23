using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using reCAPTCHA.AspNetCore;
using SaltStackers.Application.Custom;
using SaltStackers.Application.Interfaces;
using SaltStackers.Data.Context;
using SaltStackers.Domain.Models.Membership;

namespace SaltStackers.Web.Helpers.Services
{
    public static class SecurityServices
    {
        public static void AddApplicationSecurity(this IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                option.AddPolicy("DynamicPermission", policy =>
                    policy.Requirements.Add(new DynamicPermissionRequirement()));
                option.AddPolicy("Hangfire", builder =>
                {
                    builder
                        .AddAuthenticationSchemes()
                        .RequireRole("Administrator")
                        .RequireAuthenticatedUser();
                });
            });

            services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            var serviceProvider = services.BuildServiceProvider();
            var applicationService = serviceProvider.GetService<IApplicationService>();
            services.AddRecaptcha(settings =>
            {
                settings.SiteKey = applicationService?.GetSetting("ReCaptchaSiteKey");
                settings.SecretKey = applicationService?.GetSetting("ReCaptchaSecretKey");
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.LoginPath = "/Home/Index";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            services.Configure<SecurityStampValidatorOptions>(option =>
            {
                // option.ValidationInterval = TimeSpan.FromSeconds(15);
            });

            services.AddIdentity<AspNetUser, AspNetRole>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<GlobalIdentityErrorDescriber>()
                .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>();
        }
    }
}
