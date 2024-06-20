using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Services.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Queries;

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
            services.AddAutoMapper(typeof(IUnitOfWork).Assembly);
            services.AddTransient<IDonutQueryService, DonutQueryService>();
            return services;
        }
    }
}
