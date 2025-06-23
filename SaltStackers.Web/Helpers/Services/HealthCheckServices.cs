using SaltStackers.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SaltStackers.Web.Helpers.Services
{
    public static class HealthCheckServices
    {
        public static void AddHealthCheckServices(this IServiceCollection services, string connectionString)
        {
            services.AddHealthChecks()
                .AddSqlServer(connectionString,
                    "select 1",
                    failureStatus: HealthStatus.Degraded,
                    name: "SQL Server")
                .AddRedis(CacheService.RedisConnectionString, "Redis Cache");

            services.AddHealthChecksUI(setup =>
                {
                    setup.SetHeaderText("Databases - Health Checks Status");
                    setup.DisableDatabaseMigrations();
                    setup.SetEvaluationTimeInSeconds(10);
                    setup.MaximumHistoryEntriesPerEndpoint(60);
                    setup.SetApiMaxActiveRequests(1);
                    setup.AddHealthCheckEndpoint("Databases", "/health");
                })
                .AddInMemoryStorage();
        }
    }
}
