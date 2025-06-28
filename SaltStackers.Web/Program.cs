using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SaltStackers.Application.Extensions;
using SaltStackers.Data.Context;
using SaltStackers.IoC;
using SaltStackers.Web.Helpers;
using SaltStackers.Web.Helpers.Configurations;
using SaltStackers.Web.Helpers.Services;
using System.Reflection;
using System.Text;
using WatchDog;
using WatchDog.src.Enums;

namespace SaltStackers.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var version = builder.Configuration.GetSection("Version").Get<string>();
            var appConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration.GetSection("Swagger:ValidIssuer").Get<string>(),
                ValidAudience = builder.Configuration.GetSection("Swagger:ValidAudience").Get<string>(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Swagger:IssuerSigningKey").Get<string>())),
                ValidateIssuer = builder.Configuration.GetSection("Swagger:ValidateIssuer").Get<bool>(),
                ValidateAudience = builder.Configuration.GetSection("Swagger:ValidateAudience").Get<bool>(),
                ValidateLifetime = builder.Configuration.GetSection("Swagger:ValidateLifetime").Get<bool>(),
                ClockSkew = TimeSpan.Zero
            };


            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(appConnectionString)
                .UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            builder.Services.AddHangfireServer();
            builder.Services.AddApplicationSwagger(tokenValidationParameters);

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new LogAttribute());
            })
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                );

            CultureHelper.SetCulture(CultureHelper.DefaultCulture);

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            builder.Services.AddApplicationSwaggerGen(version, xmlPath);
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

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.WithOrigins(corsOrigins);
            corsBuilder.AllowCredentials();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ApiCorsPolicy", corsBuilder.Build());
            });

            builder.Services.AddWatchDogServices(opt =>
            {
                opt.SetExternalDbConnString = appConnectionString;
                opt.DbDriverOption = WatchDogDbDriverEnum.MSSQL;
            });
            builder.Logging.AddWatchDogLogger();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", $"SaltStackers {version}"));
            }

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
