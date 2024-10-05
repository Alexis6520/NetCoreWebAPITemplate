using Application.Services.Commands.Donuts.Create;
using Application.Services.Commands.Donuts.Update;
using Application.Services.DTOs.Donuts;
using Application.Services.Wrappers;
using IntegrationTests.Abstractions;
using IntegrationTests.Services;
using System.Net.Http.Json;

namespace IntegrationTests
{
    public class DonutTests(CustomWebAppFactory factory) : BaseTest(factory)
    {
        private const string BASE_URL = "Donuts";
        private readonly List<int> _toDeleteIds = [];

        [Fact]
        public async Task Create()
        {
            var command = new CreateDonutCommand
            {
                Name = "Frambuesa",
                Price = 19.99m,
                Description = "Dona de frambuesa",
            };

            var id = await CreateDonut(command);
            var donut = DbContext.Donuts.Find(id);
            Assert.NotNull(donut);
            Assert.Equal(donut.Name, command.Name);
            Assert.Equal(donut.Price, command.Price);
            Assert.Equal(donut.Description, command.Description);
        }

        private async Task<int> CreateDonut(CreateDonutCommand command = null)
        {
            command ??= new CreateDonutCommand
            {
                Name = "test",
                Price = 10,
                Description = "test",
            };

            var response = await Client.PostAsJsonAsync(BASE_URL, command);
            response.EnsureSuccessStatusCode();
            var responseModel = await response.Content.ReadFromJsonAsync<Response<int>>();
            var id = responseModel.Value;
            _toDeleteIds.Add(id);
            return id;
        }

        [Fact]
        public async Task GetAll()
        {
            for (int i = 0; i < 3; i++)
            {
                await CreateDonut();
            }

            var responseModel = await Client.GetFromJsonAsync<Response<List<DonutListItemDTO>>>(BASE_URL);
            var list = responseModel.Value;
            Assert.NotNull(list);
            Assert.True(list.Count >= 3);
        }

        [Fact]
        public async Task GetById()
        {
            var id = await CreateDonut();
            var target = DbContext.Donuts.Find(id);
            var url = $"{BASE_URL}/{target.Id}";
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var model = await response.Content.ReadFromJsonAsync<Response<DonutDTO>>();
            var dto = model.Value;
            Assert.NotNull(dto);
            Assert.Equal(dto.Name, target.Name);
            Assert.Equal(dto.Description, target.Description);
            Assert.Equal(dto.Price, target.Price);
        }

        [Fact]
        public async Task Update()
        {
            var command = new CreateDonutCommand
            {
                Name = "UpdateTest",
                Description = "Prueba de actualización de dona",
                Price = 19.99m
            };

            var id = await CreateDonut(command);

            var updateCommand = new UpdateDonutCommand
            {
                Name = "NameChanged",
                Description = "DescChanged",
                Price = 1800m
            };

            var url = $"{BASE_URL}/{id}";
            var response = await Client.PutAsJsonAsync(url, updateCommand);
            response.EnsureSuccessStatusCode();
            var donut = DbContext.Donuts.Find(id);
            Assert.Equal(donut.Name, updateCommand.Name);
            Assert.Equal(donut.Price, updateCommand.Price);
            Assert.Equal(donut.Description, updateCommand.Description);
        }

        [Fact]
        public async Task Delete()
        {
            var id = await CreateDonut();
            var url = $"{BASE_URL}/{id}";
            var response = await Client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            var exists = DbContext.Donuts.Any(x => x.Id == id);
            Assert.False(exists);
        }

        protected override void TestCleanUp()
        {
            DbContext.Donuts.RemoveRange(DbContext.Donuts.Where(x => _toDeleteIds.Contains(x.Id)));
            DbContext.SaveChanges();
        }
    }
}
