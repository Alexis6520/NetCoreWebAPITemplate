using Infrastructure.Persistence;
using IntegrationTests.Services;
using Logic.Handlers.DonutHandlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services.Queries.DTOs.DonutDTOs;
using Services.Wrappers;
using System.Net.Http.Json;

namespace IntegrationTests
{
    public class DonutTest(CustomWebAppFactory factory) : BaseTest(factory)
    {
        private const string Url = "Donuts";

        [Fact]
        public async Task UpdateDonut()
        {
            // Crear dona con la que se probará la actualización
            using var scope = _factory.Services.CreateScope();

            using var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await CreateDonutAsync();

            var id = await dbContext.Donuts
                .Select(x => x.Id)
                .FirstAsync();

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

        private async Task CreateDonutAsync()
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
        public async Task GetAll()
        {
            var size = 2;
            var url = $"{Url}?page=1&pageSize={size}";
            var result = await _client.GetFromJsonAsync<PaginatedList<DonutDTO>>(url);
            Assert.NotNull(result);

            if (result.TotalCount > 0)
            {
                Assert.True(result.Items.Count == size);
                Assert.True(result.Items.Count <= result.TotalCount);
                Assert.True(result.PageCount >= 1);
            }
            else
            {
                Assert.Equal(0, result.TotalCount);
                Assert.Equal(0, result.PageCount);
            }
        }

        [Fact]
        public async Task GetById()
        {
            using var scope = _factory.Services.CreateScope();

            using var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await CreateDonutAsync();

            var id = await dbContext.Donuts
                .Select(x => x.Id)
                .FirstAsync();

            var result = await _client.GetFromJsonAsync<Result<DonutDTO>>($"{Url}/{id}");
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
        }
    }
}
