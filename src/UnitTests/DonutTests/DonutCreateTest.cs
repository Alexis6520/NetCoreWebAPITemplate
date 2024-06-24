using Domain.Entities;
using Logic.Handlers.DonutHandlers;
using Moq;
using System.Net;

namespace UnitTests.DonutTests
{
    [TestClass]
    public class DonutCreateTest : BaseTest<DonutCreateHandler>
    {
        [TestMethod]
        public async Task CreateDonut()
        {
            // Configurar mock de UnitOfWork
            _unitOfWorkMock.Setup(x => x.Donuts.AddAsync(It.IsAny<Donut>(), default))
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default))
                .Verifiable();

            // Simular operación
            var command = new DonutCreateCommand("Glaseada original", 19);
            var handler = new DonutCreateHandler(_unitOfWorkMock.Object, _loggerMock.Object);
            var result = await handler.Handle(command, default);

            // Verificar la ejecución de los métodos
            _unitOfWorkMock.VerifyAll();

            // Comprobar resultado
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        }
    }
}
