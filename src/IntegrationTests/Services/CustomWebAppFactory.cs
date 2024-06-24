using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace IntegrationTests.Services
{
    /// <summary>
    /// Fabrica personalizada de API Web
    /// </summary>
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        public const string ConnectionString = "Server=10.10.50.5;Database=NetCoreWebAPITemplateTest;User=sa;Password=@dministrator1;TrustServerCertificate=True;";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(SetPersistence);
            builder.UseEnvironment("Development");
        }

        private static void SetPersistence(IServiceCollection services)
        {
            var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));

            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            services.AddDbContext<ApplicationDbContext>((container, options) =>
            {
                options.UseSqlServer(ConnectionString);
            });
        }

        /// <summary>
        /// Crea un cliente personalizado para pruebas
        /// </summary>
        /// <returns></returns>
        public HttpClient CreateCustomClient()
        {
            var client = WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.Configure<AuthenticationOptions>(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                        options.DefaultScheme = "TestScheme";
                    });

                    services.AddAuthentication(defaultScheme: "TestScheme")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "TestScheme", options => { });
                });
            }).CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
            return client;
        }
    }
}
