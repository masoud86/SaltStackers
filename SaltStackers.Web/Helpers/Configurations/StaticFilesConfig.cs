using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace SaltStackers.Web.Helpers.Configurations
{
    public static class StaticFilesConfig
    {
        public static void UseStaticFilesConfig(this IApplicationBuilder app)
        {
            app.UseStaticFiles();
            //var provider = new FileExtensionContentTypeProvider();
            //provider.Mappings[".mrt"] = "application/json";
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports")),
            //    RequestPath = "/reports",
            //    ContentTypeProvider = provider
            //});
        }
    }
}
