using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.SQLServer
{
    public class SQLServerContext(IConfiguration configuration) : ApplicationDbContext(configuration)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Configuration.GetConnectionString("Main");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
