using Hangfire;
using Hangfire.Dashboard;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace SaltStackers.Web.Helpers.Configurations
{
    public static class EndpointsConfig
    {
        public static void UseEndpointsConfig(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapHangfireDashboard();
                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions()
                {
                    Authorization = Enumerable.Empty<IDashboardAuthorizationFilter>(),
                    DashboardTitle = "SaltStackers"                    
                })
                .RequireAuthorization("Hangfire");
                //endpoints.MapHealthChecks("/health",
                //    new HealthCheckOptions
                //    {
                //        Predicate = _ => true,
                //        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                //    });
                //endpoints.MapHealthChecksUI(options =>
                //    {
                //        options.UseRelativeApiPath = true;
                //        options.UIPath = "/monitoring";
                //    })
                //    .RequireAuthorization(new AuthorizeAttribute { Roles = "Administrator" });
            });
        }
    }
}
