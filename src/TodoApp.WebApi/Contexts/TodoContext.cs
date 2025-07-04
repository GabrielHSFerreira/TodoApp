using Microsoft.EntityFrameworkCore;
using TodoApp.WebApi.Domain;

namespace TodoApp.WebApi.Contexts
{
    public class TodoContext : DbContext
    {
        public DbSet<Todo> Todos => Set<Todo>();

        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
    }
}