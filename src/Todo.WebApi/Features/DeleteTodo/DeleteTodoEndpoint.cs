using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoIntegrationTests.WebApi.Contexts;

namespace TodoIntegrationTests.WebApi.Features.DeleteTodo
{
    public class DeleteTodoEndpoint : IEndpoint
    {
        public void Register(IEndpointRouteBuilder builder)
        {
            builder.MapDelete("/todos/{id:int}", DeleteTodo);
        }

        private static async Task<NoContent> DeleteTodo(int id, TodoContext context, CancellationToken cancellationToken)
        {
            await context.Todos.Where(todo => todo.Id == id).ExecuteDeleteAsync(cancellationToken);
            return TypedResults.NoContent();
        }
    }
}