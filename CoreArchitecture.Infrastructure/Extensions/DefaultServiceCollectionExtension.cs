using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreArchitecture.Infrastructure.Extensions
{
    public static class DefaultServiceCollectionExtension
    {
        public static IApplicationBuilder UseDefaultServices(this IApplicationBuilder app, bool autoMigration = false)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (autoMigration)
            {
                using var scope = app.ApplicationServices.CreateScope();
                var serviceProvider = scope.ServiceProvider;
                DatabaseInitializer.MigrateDataBase(serviceProvider);
            }

            return app;
        }
    }
}