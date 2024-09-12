using Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Host.Extensions
{
    public static class WebApplicationExtension
    {
        /// <summary>
        /// Inicializa la base de datos
        /// </summary>
        /// <param name="webApplication"></param>
        public static void InitializeDatabase(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
