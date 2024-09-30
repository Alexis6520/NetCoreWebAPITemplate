using Application.Services.Commands;
using Application.Services.Commands.Donuts;
using Domain.Entities;
using System.Net;
using UnitTests.Abstractions;

namespace UnitTests.Donuts
{
    [TestClass]
    public class DeleteDonutTest : BaseTest<DeleteDonutHandler>
    {
        [TestInitialize]
        public void Initialize()
        {
            var donut = new Donut("Glaseada original", 19.99m)
            {
                Description = "La original"
            };

            DbContext.Donuts.Add(donut);
            DbContext.SaveChanges();
        }

        [TestMethod]
        public async Task Delete()
        {
            var id = DbContext.Donuts
                .Select(x => x.Id)
                .First();

            var command = new DeleteCommand<int, Donut>(id);
            var handler = new DeleteDonutHandler(DbContext, LoggerMock.Object);
            var response = await handler.Handle(command, default);
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            var exists = DbContext.Donuts.Any(x => x.Id == id);
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public async Task DeleteNonExistingDonut()
        {
            var command = new DeleteCommand<int, Donut>(1024);
            var handler = new DeleteDonutHandler(DbContext, LoggerMock.Object);
            var response = await handler.Handle(command, default);
            Assert.IsFalse(response.Succeeded);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
