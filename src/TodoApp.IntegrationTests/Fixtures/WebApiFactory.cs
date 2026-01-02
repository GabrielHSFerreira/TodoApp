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
            _postgreSqlContainer = new PostgreSqlBuilder("postgres:18.1").Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("ConnectionStrings:Database", _postgreSqlContainer.GetConnectionString());
        }

        public async ValueTask InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();
        }

        public override async ValueTask DisposeAsync()
        {
            await _postgreSqlContainer.DisposeAsync();
            await base.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}