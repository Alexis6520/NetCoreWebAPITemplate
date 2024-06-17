using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Logic
{
    /// <summary>
    /// Extensiones para inyectar servicios de Lógica de negocio
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Agrega los servicios de lógica de negocio
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
            return services;
        }
    }
}
