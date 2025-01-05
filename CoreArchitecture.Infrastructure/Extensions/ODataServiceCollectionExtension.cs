using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Reflection;
using CoreArchitecture.Common.Constants;

namespace CoreArchitecture.Infrastructure.Extensions
{
    public static class ODataServiceCollectionExtension
    {
        public static IServiceCollection AddODataServices(this IServiceCollection services)
        {
            var defaultBatchHandler = new DefaultODataBatchHandler();
            defaultBatchHandler.MessageQuotas.MaxOperationsPerChangeset = 10;
            services.AddControllers().AddNewtonsoftJson().AddOData(options =>
            {
                options.Expand().Select().Filter().OrderBy().Count().SetMaxTop(null).AddRouteComponents(
                routePrefix: ApplicationConstants.ODataRoutePrefix,
                model: GetEdmModel(),
                batchHandler: defaultBatchHandler);
            });

            return services;
        }
        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EnableLowerCamelCase();

            var assembly = Assembly.Load("CoreArchitecture.Common");

            var types = assembly.GetTypes()
                    .Where(t => t.IsClass
                             && !t.IsAbstract
                             && t.Namespace == "CoreArchitecture.Common.DTOs")
                    .ToList();
            MethodInfo? entitySetMethod = builder.GetType().GetMethod("EntitySet", new Type[] { typeof(string) });
            foreach (var type in types)
            {
                MethodInfo? genericEntitySetMethod = entitySetMethod?.MakeGenericMethod(type);
                genericEntitySetMethod?.Invoke(builder, new object[] { type.Name.Replace("Dto", "") });
            }

            return builder.GetEdmModel();
        }
    }
}