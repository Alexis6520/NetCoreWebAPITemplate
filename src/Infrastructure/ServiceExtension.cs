using Domain.Services;
using Infrastructure.Persistence.SQLServer;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Agrega los servicios de infraestructura
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDbContext, SQLServerContext>();
            return services;
        }
    }
}
