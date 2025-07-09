using Microsoft.EntityFrameworkCore;
using TodoApp.WebApi.Contexts;
using TodoApp.WebApi.Domain;

namespace TodoApp.IntegrationTests.Todos
{
    internal static class TodosHelper
    {
        public static Task<bool> TodoExists(this TodoContext context, int id)
        {
            return context.Todos.AnyAsync(t => t.Id == id);
        }

        public static async Task<Todo> CreateTodo(this TodoContext context)
        {
            var newTodo = new Todo("Test", "Test");
            context.Todos.Add(newTodo);
            await context.SaveChangesAsync();

            return newTodo;
        }
    }
}