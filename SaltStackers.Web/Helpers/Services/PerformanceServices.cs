using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using WebMarkupMin.AspNetCore5;

namespace SaltStackers.Web.Helpers.Services
{
    public static class PerformanceServices
    {
        public static void AddPerformance(this IServiceCollection services)
        {
            services.AddWebMarkupMin(options =>
            {
                options.AllowCompressionInDevelopmentEnvironment = true;
                options.AllowMinificationInDevelopmentEnvironment = true;
            })
                .AddHtmlMinification(options =>
                {
                    options.MinificationSettings.RemoveRedundantAttributes = true;
                    options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                    options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
                    //options.ExcludedPages = new List<IUrlMatcher>
                    //{
                    //    new ExactUrlMatcher("/Monitoring"),
                    //    new ExactUrlMatcher("/Monitoring#/healthchecks"),
                    //    new WildcardUrlMatcher("/Monitoring*")
                    //};
                })
                .AddHttpCompression();

            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>
            (opt =>
            {
                opt.Level = CompressionLevel.Fastest;
            });
        }
    }
}
