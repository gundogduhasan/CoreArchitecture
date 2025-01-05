using CoreArchitecture.Infrastructure.Filters.SwaggerFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreArchitecture.Infrastructure.Extensions
{
    public static class SwaggerServiceCollectionExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<ODataDocumentFilter>();
                c.OperationFilter<ODataParameterFilter>();
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerServices(this IApplicationBuilder app, bool isDevelopment)
        {
            if (isDevelopment)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    }
}