using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Domain.Services
{
    /// <summary>
    /// Abstracción del contexto de base de datos de la aplicación
    /// </summary>
    /// <param name="configuration">Configuración</param>
    public abstract class ApplicationDbContext(IConfiguration configuration) : DbContext
    {
        /// <summary>
        /// Configuración de la aplicación
        /// </summary>
        protected readonly IConfiguration Configuration = configuration;

        /// <summary>
        /// Donas
        /// </summary>
        public DbSet<Donut> Donuts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var type = GetType();
            var assembly = type.Assembly;
            var configurationNamespace = $"{type.Namespace}.Configuration";
            modelBuilder.ApplyConfigurationsFromAssembly(assembly, configType => configType.Namespace == configurationNamespace);
        }
    }
}
