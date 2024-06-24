using IntegrationTests.Services;

namespace IntegrationTests
{
    public abstract class BaseTest(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>, IDisposable
    {
        protected readonly CustomWebAppFactory _factory = factory;
        protected HttpClient _client = factory.CreateCustomClient();

        public virtual void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}