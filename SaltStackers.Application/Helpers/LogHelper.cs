using System;
using SaltStackers.Application.ViewModels.Log;
using Microsoft.AspNetCore.Http;
using UAParser;

namespace SaltStackers.Application.Helpers
{
    public static class LogHelper
    {
        private static string GetUserIpAddress(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }

        public static string GroupKey() => DateTime.UtcNow.ToString("yyyyMMddHHmmssff");

        public static ClientInformation GetClientInfo(this HttpRequest request)
        {
            var userAgent = request.Headers["User-Agent"];
            var userAgentParser = Parser.GetDefault();
            var client = userAgentParser.Parse(userAgent);
            return new ClientInformation
            {
                Device = client.Device.Family,
                Browser = client.UA.Family,
                BrowserVersion = $"{client.UA.Major}.{client.UA.Minor}.{client.UA.Patch}",
                OperatingSystem = client.OS.ToString(),
                IpAddress = GetUserIpAddress(request.HttpContext)
            };
        }
    }
}
