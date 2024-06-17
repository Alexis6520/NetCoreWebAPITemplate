using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace WebAPI
{
    public static class Startup
    {
        public static void InitializeDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            using var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
