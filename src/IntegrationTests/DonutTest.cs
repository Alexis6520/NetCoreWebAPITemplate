using Domain.Entities;
using Infrastructure.Persistence;
using IntegrationTests.Services;
using Logic.Handlers.DonutHandlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace IntegrationTests
{
    public class DonutTest(CustomWebAppFactory factory) : BaseTest(factory)
    {
        private const string Url = "Donuts";

        [Fact]
        public async Task CreateDonut()
        {
            // Ejecutar
            var command = new DonutCreateCommand("Frambuesa", 19);
            var response = await _client.PostAsJsonAsync(Url, command);
            response.EnsureSuccessStatusCode();

            // Comprobar que la dona esté en la base de datos 
            using var scope = _factory.Services.CreateScope();

            using var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var exists = await dbContext.Donuts
                .AnyAsync(x => x.Name == command.Name);

            Assert.True(exists);
        }

        [Fact]
        public async Task UpdateDonut()
        {
            // Crear dona con la que se probará la actualización
            using var scope = _factory.Services.CreateScope();

            using var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var id = await CreateTestDonutAsync(dbContext);

            // Actualizar por medio de la API
            var command = new DonutUpdateCommand
            {
                Name = "Manzana",
                Price = 10
            };

            var url = $"{Url}/{id}";
            var response = await _client.PutAsJsonAsync(url, command);
            response.EnsureSuccessStatusCode();

            // Recuperar dona y comprobar que se haya actualizado
            var donut = await dbContext.Donuts
                .FindAsync(id);

            Assert.NotNull(donut);
            Assert.Equal(command.Name, donut.Name);
            Assert.Equal(command.Price, donut.Price);
        }

        private static async Task<int> CreateTestDonutAsync(ApplicationDbContext dbContext)
        {
            var donut = new Donut("Chocolate", 19);
            await dbContext.Donuts.AddAsync(donut);
            await dbContext.SaveChangesAsync();
            dbContext.ChangeTracker.Clear();
            return donut.Id;
        }
    }
}
