using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace Infrastructure
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Agrega las implementaciones de los servicios
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
