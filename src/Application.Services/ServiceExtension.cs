using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Services
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Agrega los servicios de aplicación
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
            return services;
        }
    }
}
