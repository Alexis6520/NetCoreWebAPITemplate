using Application.Services.Commands.Donuts.Create;
using System.Net;
using UnitTests.Abstractions;

namespace UnitTests.Donuts
{
    [TestClass]
    public class CreateDonutTest : BaseTest<CreateDonutHandler>
    {
        [TestMethod]
        public async Task Create()
        {
            var command = new CreateDonutCommand
            {
                Name = "Frambuesa",
                Description = "Donita de frambuesa",
                Price = 19.99m
            };

            var handler = new CreateDonutHandler(DbContext, LoggerMock.Object);
            var response = await handler.Handle(command, default);
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Assert.IsTrue(DbContext.Donuts.Any(x => x.Id == response.Value));
        }
    }
}
