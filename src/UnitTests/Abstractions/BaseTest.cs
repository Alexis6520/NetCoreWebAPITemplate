using Moq;
using Microsoft.Extensions.Logging;
using Domain.Services;
using UnitTests.Services;

namespace UnitTests.Abstractions
{
    public abstract class BaseTest<THandler> : IDisposable
    {
        protected Mock<ILogger<THandler>> LoggerMock = new();
        private MemoryDbContext _dbContext;

        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = new MemoryDbContext($"{typeof(THandler).Name}Database");
                    _dbContext.Database.EnsureDeleted();
                    _dbContext.Database.EnsureCreated();
                }

                return _dbContext;
            }
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
            _dbContext = null;
            LoggerMock = null;
            GC.SuppressFinalize(this);
        }
    }
}