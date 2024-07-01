using Domain.Entities;
using Logic.Handlers.DonutHandlers;
using Moq;
using Services.Wrappers;
using System.Net;

namespace UnitTests.DonutTests
{
    [TestClass]
    public class DonutDeleteTest : BaseTest<DonutDeleteHandler>
    {
        [TestMethod]
        public async Task Delete()
        {
            // Configurar UnitOfwork
            var donut = new Donut("Chocolate", 19);

            _unitOfWorkMock.Setup(x => x.Donuts.FindAsync(It.IsAny<object[]>(), default))
                .ReturnsAsync(donut);

            _unitOfWorkMock.Setup(x => x.Donuts.Remove(donut))
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default))
                .Verifiable();

            // Simular operación
            var command = new DonutDeleteCommand(1);
            var handler = new DonutDeleteHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            var result = await handler.Handle(command, default);

            // Verificar métodos
            _unitOfWorkMock.VerifyAll();

            // Comprobar resultado
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task DeleteNonExistentDonut()
        {
            // Configurar UnitOfwork
            Donut donut = null;

            _unitOfWorkMock.Setup(x => x.Donuts.FindAsync(It.IsAny<object[]>(), default))
                .ReturnsAsync(donut);

            // Simular operación
            var command = new DonutDeleteCommand(1);
            var handler = new DonutDeleteHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            var result = await handler.Handle(command, default);

            // Comprobar resultado
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Any());
        }
    }
}
