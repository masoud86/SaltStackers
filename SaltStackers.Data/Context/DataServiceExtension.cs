using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SaltStackers.Data.Context
{
    public static class DataServiceExtension
    {
        public static void AddProjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options
                .UseSqlServer(connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure();
                        sqlOptions.MigrationsAssembly("SaltStackers.Data");
                    });
                //options.EnableSensitiveDataLogging();
            });

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
        }
    }
}
