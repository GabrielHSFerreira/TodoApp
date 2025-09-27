using Microsoft.EntityFrameworkCore;
using TodoApp.WebApi.Contexts;
using TodoApp.WebApi.Domain;

namespace TodoApp.IntegrationTests.Todos
{
    internal static class TodosHelper
    {
        public static Task<bool> TodoExists(this TodoContext context, int id, CancellationToken cancellationToken)
        {
            return context.Todos.AnyAsync(t => t.Id == id);
        }

        public static async Task<Todo> CreateTodo(this TodoContext context, CancellationToken cancellationToken)
        {
            var newTodo = new Todo("Test", "Test");
            context.Todos.Add(newTodo);
            await context.SaveChangesAsync(cancellationToken);

            return newTodo;
        }

        public static async Task<Todo?> FindTodo(this TodoContext context, int id, CancellationToken cancellationToken)
        {
            var todo = await context.Todos.FindAsync(id, cancellationToken);
            return todo;
        }
    }
}