using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using TodoApp.IntegrationTests.Fixtures;
using TodoApp.WebApi.Contexts;

[assembly: AssemblyFixture(typeof(PostgreSqlDatabase))]

namespace TodoApp.IntegrationTests.Fixtures
{
    public class PostgreSqlDatabase : IAsyncLifetime
    {
        public readonly PostgreSqlContainer Container;

        public PostgreSqlDatabase()
        {
            Container = new PostgreSqlBuilder("postgres:18.1").Build();
        }

        public async ValueTask InitializeAsync()
        {
            await Container.StartAsync();

            var context = new TodoContext(
                new DbContextOptionsBuilder<TodoContext>().UseNpgsql(Container.GetConnectionString()).Options);

            await context.Database.EnsureCreatedAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await Container.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}