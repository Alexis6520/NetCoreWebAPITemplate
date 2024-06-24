using Domain.Entities;
using Logic.Handlers.DonutHandlers;
using Moq;
using System.Net;

namespace UnitTests.DonutTests
{
    [TestClass]
    public class DonutUpdateTest : BaseTest<DonutUpdateHandler>
    {
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
            Assert.AreEqual(donut.Name,command.Name);
            Assert.AreEqual(donut.Price,command.Price);
        }
    }
}
