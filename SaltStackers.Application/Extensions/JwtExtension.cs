using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace SaltStackers.Application.Extensions;
public static class JwtExtension
{
    public static void AddApplicationSwagger(this IServiceCollection services, TokenValidationParameters parameters)
    {
        services
        .AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.TokenValidationParameters = parameters;
            cfg.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject("Invalid credentials; The provided authentication credentials were invalid.");
                    return context.Response.WriteAsync(result);
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject("Forbidden; You don’t have permission to access.");
                    return context.Response.WriteAsync(result);

                    //context.Response.StatusCode = 403;
                    //context.Response.ContentType = "application/json";
                    //var result = JsonConvert.SerializeObject("Forbidden; You don’t have permission to access.");
                    //return context.Response.WriteAsync(result);
                }
            };
        });
    }

    public static void AddApplicationSwaggerGen(this IServiceCollection services, string version, string xmlPath)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v2", new OpenApiInfo { Title = "SaltStackers", Version = $"v2" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme
                        {
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"},
                         Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header
                        },
                    new string[] {}
                }
            });

            c.IncludeXmlComments(xmlPath);
        });
    }
}
