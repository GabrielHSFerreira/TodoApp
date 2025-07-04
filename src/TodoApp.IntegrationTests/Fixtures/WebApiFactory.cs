using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TodoApp.WebApi;
using TodoApp.WebApi.Contexts;

namespace TodoApp.IntegrationTests.Fixtures
{
    public class WebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public TodoContext Context => Services
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<TodoContext>();

        private readonly PostgreSqlContainer _postgreSqlContainer;

        public WebApiFactory()
        {
            _postgreSqlContainer = new PostgreSqlBuilder().Build();
        }

        Task IAsyncLifetime.InitializeAsync()
        {
            return _postgreSqlContainer.StartAsync();
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("ConnectionStrings:Database", _postgreSqlContainer.GetConnectionString());
        }
    }
}