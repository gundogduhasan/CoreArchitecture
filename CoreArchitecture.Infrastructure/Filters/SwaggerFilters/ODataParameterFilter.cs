using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CoreArchitecture.Infrastructure.Filters.SwaggerFilters
{
    public class ODataParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            if (context.ApiDescription?.RelativePath != null && context.ApiDescription.RelativePath.Contains("({key})"))
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "key",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "The key for this OData resource. It can be a string (e.g., GUID) or an integer."
                });
            }
        }
    }
}