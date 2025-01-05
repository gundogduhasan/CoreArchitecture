using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CoreArchitecture.Infrastructure.Filters.SwaggerFilters
{
    public class ODataDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var path in swaggerDoc.Paths.ToList())
            {
                foreach (var operation in path.Value.Operations)
                    operation.Value.OperationId += $"_{operation.Key.ToString().ToLower()}";
                
                if (path.Key.Contains("/{key}"))
                    swaggerDoc.Paths.Remove(path.Key);
            }
        }
    }
}