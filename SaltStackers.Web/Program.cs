using SaltStackers.Data.Context;
using SaltStackers.IoC;
using SaltStackers.Web.Helpers;
using SaltStackers.Web.Helpers.Configurations;
using SaltStackers.Web.Helpers.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WatchDog;
using WatchDog.src.Enums;

namespace SaltStackers.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var appConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");


            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(appConnectionString)
                .UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            builder.Services.AddHangfireServer();

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new LogAttribute());
            })
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                );

            CultureHelper.SetCulture(CultureHelper.DefaultCulture);

            builder.Services.AddMemoryCache();
            builder.Services.AddTransient<IUtilities, Utilities>();
            builder.Services.AddScoped<IAuthorizationHandler, DynamicPermissionHandler>();
            builder.Services.RegisterServices();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddProjectDbContext(appConnectionString);
            builder.Services.AddApplicationSecurity();
            builder.Services.AddPerformance();

            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "ksljflskdjfkls";
                googleOptions.ClientSecret = "sdjkflksdjf;asdf";
            });

            builder.Services.AddWatchDogServices(opt =>
            {
                opt.SetExternalDbConnString = appConnectionString;
                opt.DbDriverOption = WatchDogDbDriverEnum.MSSQL;
            });
            builder.Logging.AddWatchDogLogger();

            var app = builder.Build();

            app.UseWatchDogExceptionLogger();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                context.Response.Headers.Add("Feature-Policy", "camera 'none'; geolocation 'none'; microphone 'none'; usb 'none'");
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'none';script-src 'self' 'unsafe-inline' 'unsafe-eval' www.google.com/recaptcha/ https://www.gstatic.com/recaptcha/;style-src 'self' 'unsafe-inline';img-src * 'self' data: https: blob:;font-src 'self' data:;connect-src *;form-action 'self'; frame-src www.google.com/recaptcha/; manifest-src 'self'; style-src-elem 'self' 'unsafe-inline' https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css");
                await next();
            });

            app.UseErrorHandlingConfig(app.Environment);
            app.UseHttpsRedirection();
            app.UseStaticFilesConfig();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpointsConfig();

            app.Run();
        }
    }
}
