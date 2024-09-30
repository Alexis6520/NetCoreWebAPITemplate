using Application.Services.Commands.Donuts.Update;
using Domain.Entities;
using System.Net;
using UnitTests.Abstractions;

namespace UnitTests.Donuts
{
    [TestClass]
    public class UpdateDonutTest : BaseTest<UpdateDonutHandler>
    {
        #region "ValidationData"
        public static IEnumerable<object[]> ValidationData =>
        [
            [new UpdateDonutCommand()],
            [new UpdateDonutCommand() { Name=""}],
            [new UpdateDonutCommand() { Name=new string('a',31)}],
            [new UpdateDonutCommand() {
                Name="Frambuesa",
                Price=-0.0000001m
            }],
            [new UpdateDonutCommand() {
                Name="Frambuesa",
                Description=new string('a',513)
            }],
        ];
        #endregion

        [TestMethod]
        [DynamicData(nameof(ValidationData))]
        public void ValidateCommand(UpdateDonutCommand command)
        {
            var validator = new UpdateDonutValidator();
            var result = validator.Validate(command);
            Assert.IsFalse(result.IsValid);
        }

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
        public async Task Update()
        {
            var donut = DbContext.Donuts.First();

            var command = new UpdateDonutCommand
            {
                Id = donut.Id,
                Name = "Gansito",
                Description = "Sabor gansito",
                Price = 17.99m
            };

            var handler = new UpdateDonutHandler(DbContext, LoggerMock.Object);
            var response = await handler.Handle(command, default);
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(donut.Name, command.Name);
            Assert.AreEqual(donut.Description, command.Description);
            Assert.AreEqual(donut.Price, command.Price);
        }

        [TestMethod]
        public async Task UpdateNonExistingDonut()
        {
            var command = new UpdateDonutCommand
            {
                Id = 1024,
                Name = "Gansito",
                Description = "Sabor gansito",
                Price = 17.99m
            };

            var handler = new UpdateDonutHandler(DbContext, LoggerMock.Object);
            var response = await handler.Handle(command, default);
            Assert.IsFalse(response.Succeeded);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
