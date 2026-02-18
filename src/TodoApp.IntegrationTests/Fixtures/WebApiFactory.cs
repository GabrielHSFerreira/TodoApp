using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.WebApi;
using TodoApp.WebApi.Contexts;

namespace TodoApp.IntegrationTests.Fixtures
{
    public class WebApiFactory : WebApplicationFactory<Program>
    {
        public TodoContext Context => Services
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<TodoContext>();

        private readonly PostgreSqlDatabase _database;

        public WebApiFactory(PostgreSqlDatabase postgreSqlContainer)
        {
            _database = postgreSqlContainer ?? throw new ArgumentNullException(nameof(postgreSqlContainer));
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("ConnectionStrings:Database", _database.Container.GetConnectionString());
        }
    }
}