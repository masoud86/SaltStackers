using SaltStackers.Application.Custom;
using SaltStackers.Application.Extensions;
using SaltStackers.Data.Context;
using SaltStackers.Domain.Models.Membership;
using SaltStackers.IoC;
using Hangfire;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using WatchDog;
using WatchDog.src.Enums;

namespace SaltStackers.Api;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var version = builder.Configuration.GetSection("Version").Get<string>();
        var developmentMode = builder.Configuration.GetSection("DevelopmentMode").Get<bool>();
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
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(appConnectionString)
                .UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

        builder.Services.AddHangfireServer();
        builder.Services.AddApplicationSwagger(tokenValidationParameters);
        builder.Services.AddControllers(options =>
        {
            options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            }));
        }).AddJsonOptions(option =>
        {
            option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        builder.Services.AddApplicationSwaggerGen(version, xmlPath);
        builder.Services.AddMemoryCache();
        builder.Services.RegisterServices();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddProjectDbContext(appConnectionString);
        builder.Services.AddHsts(options =>
        {
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        });

        builder.Services.Configure<SecurityStampValidatorOptions>(option =>
        {
            // option.ValidationInterval = TimeSpan.FromSeconds(15);
        });

        builder.Services.AddIdentity<AspNetUser, AspNetRole>(options =>
        {
            options.User.RequireUniqueEmail = false;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<GlobalIdentityErrorDescriber>()
            .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>();

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
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        if (developmentMode)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", $"SaltStackers {version}"));
        }

        app.UseWatchDogExceptionLogger();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("ApiCorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseWatchDog(opt =>
        {
            opt.WatchPageUsername = "loguser";
            opt.WatchPagePassword = "123456";
            opt.Blacklist = "Customer/Login,Customer/Register,Customer/Refresh,Payment/PayByCard";
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.Run();
    }
}
