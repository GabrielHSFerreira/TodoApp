using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoApp.WebApi.Contexts;

namespace TodoApp.WebApi.Features.GetTodo
{
    public class GetTodoEndpoint : IEndpoint
    {
        public void Register(IEndpointRouteBuilder builder)
        {
            builder.MapGet("/todos/{id:int}", GetTodo).WithTags(EndpointTags.Todos);
        }

        private static async Task<Results<Ok<GetTodoResponse>, NotFound>> GetTodo(
            int id,
            TodoContext context,
            CancellationToken cancellationToken)
        {
            var todo = await context.Todos
                .Where(x => x.Id == id)
                .Select(x => new GetTodoResponse(
                    x.Id,
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.Title,
                    x.Description,
                    x.Status))
                .FirstOrDefaultAsync(cancellationToken);

            if (todo == null)
                return TypedResults.NotFound();

            return TypedResults.Ok(todo);
        }
    }
}