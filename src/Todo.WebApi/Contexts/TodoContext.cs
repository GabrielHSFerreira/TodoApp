using Microsoft.EntityFrameworkCore;
using TodoIntegrationTests.WebApi.Domain;

namespace TodoIntegrationTests.WebApi.Contexts
{
    public class TodoContext : DbContext
    {
        public DbSet<Todo> Todos => Set<Todo>();

        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
    }
}