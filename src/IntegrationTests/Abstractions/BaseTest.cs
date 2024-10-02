using Domain.Services;
using IntegrationTests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Abstractions
{
    public abstract class BaseTest(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>, IDisposable
    {
        private CustomWebAppFactory _factory = factory;
        private HttpClient _client;
        private ApplicationDbContext _dbContext;
        private IServiceScope _scope = factory.Services.CreateScope();

        protected HttpClient Client => _client ??= _factory.CreateClient();
        protected ApplicationDbContext DbContext => _dbContext ??= _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        protected virtual void TestCleanUp()
        {
        }

        public void Dispose()
        {
            TestCleanUp();
            _factory.Dispose();
            _factory = null;
            _client.Dispose();
            _client = null;
            _dbContext?.Dispose();
            _dbContext = null;
            _scope.Dispose();
            _scope = null;
            GC.SuppressFinalize(this);
        }
    }
}
