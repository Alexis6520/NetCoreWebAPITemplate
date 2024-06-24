using Microsoft.Extensions.Logging;
using Moq;
using Services;

namespace UnitTests
{
    public abstract class BaseTest<THandler> where THandler : class
    {
        protected Mock<ILogger<THandler>> _loggerMock = new ();
        protected Mock<IUnitOfWork> _unitOfWorkMock = new();
    }
}