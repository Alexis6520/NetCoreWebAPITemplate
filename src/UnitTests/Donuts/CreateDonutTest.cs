using Application.Services.Commands.Donuts.Create;
using System.Net;
using UnitTests.Abstractions;

namespace UnitTests.Donuts
{
    [TestClass]
    public class CreateDonutTest : BaseTest<CreateDonutHandler>
    {
        #region "ValidationData"
        public static IEnumerable<object[]> ValidationData =>
        [
            [new CreateDonutCommand()],
            [new CreateDonutCommand() { Name=""}],
            [new CreateDonutCommand() { Name=new string('a',31)}],
            [new CreateDonutCommand() {
                Name="Frambuesa",
                Price=-0.0000001m
            }],
            [new CreateDonutCommand() {
                Name="Frambuesa",
                Description=new string('a',513)
            }],
        ];
        #endregion

        [TestMethod]
        [DynamicData(nameof(ValidationData))]
        public void ValidateCommand(CreateDonutCommand command)
        {
            var validator = new CreateDonutValidator();
            var result = validator.Validate(command);
            Assert.IsFalse(result.IsValid);
        }

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
