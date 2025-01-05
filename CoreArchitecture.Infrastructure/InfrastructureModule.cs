using CoreArchitecture.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreArchitecture.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddODataServices();
            services.AddSwaggerServices();

            return services;
        }
        public static IApplicationBuilder UseInfrastructureMiddleware(this IApplicationBuilder app, bool isDevelopment, bool autoMigration=false)
        {
            app.UseSwaggerServices(isDevelopment);
            app.UseDefaultServices(autoMigration);

            return app;
        }
    }
}
