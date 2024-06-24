using Domain.Entities;
using Logic.Handlers.DonutHandlers;
using Logic.Validators.DonutValidators;
using Moq;
using System.Net;

namespace UnitTests.DonutTests
{
    [TestClass]
    public class DonutUpdateTest : BaseTest<DonutUpdateHandler>
    {
        public static IEnumerable<object[]> ValidationData
        {
            get
            {
                return
                [
                    [new DonutUpdateCommand{
                        Name="",
                        Price=1
                    }],
                     [new DonutUpdateCommand{
                        Name="",
                        Price=-1
                    }],
                    [new DonutUpdateCommand{
                        Name="Glaseada original",
                        Price=-1
                    }]
                ];
            }
        }

        [TestMethod]
        public async Task UpdateDonut()
        {
            // Configurar UnitOfWork
            var donut = new Donut("Frambuesa", 19);

            _unitOfWorkMock.Setup(x => x.Donuts.FindAsync(It.IsAny<object[]>(), default))
                .ReturnsAsync(donut);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default))
                .Verifiable();

            // Simular operación
            var command = new DonutUpdateCommand
            {
                Id = 1,
                Name = "Chocolate",
                Price = 10
            };

            var handler = new DonutUpdateHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            var result = await handler.Handle(command, default);

            // Verificar métodos
            _unitOfWorkMock.VerifyAll();

            // Comprobar resultado
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);

            // Comprobar actualización
            Assert.AreEqual(donut.Name, command.Name);
            Assert.AreEqual(donut.Price, command.Price);
        }

        [TestMethod]
        public async Task UpdateNonExistent()
        {
            // Configurar UnitOfWork
            _unitOfWorkMock.Setup(x => x.Donuts.FindAsync(It.IsAny<object[]>(), default))
                .Returns(Task.FromResult<Donut?>(null));

            // Simular operación
            var command = new DonutUpdateCommand();
            var handler = new DonutUpdateHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            var result = await handler.Handle(command, default);

            // Comprobar resultado
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Any());
        }

        [TestMethod]
        [DynamicData(nameof(ValidationData))]
        public void ValidateCommand(DonutUpdateCommand command)
        {
            var validator = new DonutUpdateValidator();
            var result = validator.Validate(command);
            Assert.IsFalse(result.IsValid);
        }
    }
}
