using CoreArchitecture.Abstractions.DataAccessInterfaces;
using CoreArchitecture.Business.Mapping;
using CoreArchitecture.Data.DBContext;
using CoreArchitecture.DataAccess;
using CoreArchitecture.DataAccess.Repository;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CoreArchitecture.Business.DIExtensions
{
    public static class BusinessServiceCollectionExtension
    {
        /// <summary>
        /// Adds Data Access and Business dependencies to DI container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessAndDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrWhiteSpace(connectionString))
                AddDataAccessServices(services, connectionString);

            var assembly = Assembly.Load("CoreArchitecture.Business");

            var types = assembly.GetTypes()
                                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("EntityService"))
                                .ToList();

            foreach (var implementationType in types)
            {
                var interfaceType = implementationType.GetInterface($"I{implementationType.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, implementationType);
                }
            }

            services.AddAutoMapper(cfg =>
            {
                cfg.AddExpressionMapping();
                cfg.AddProfile(typeof(MappingProfile));
            });

            return services;
        }
        private static IServiceCollection AddDataAccessServices(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<CoreArchitectureDBContext>(options =>
                    options.UseSqlServer(connectionString)
            );

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
