using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using TodoApp.WebApi.Contexts;

namespace TodoApp.WebApi.Features.UpdateTodo
{
    public class UpdateTodoEndpoint : IEndpoint
    {
        public void Register(IEndpointRouteBuilder builder)
        {
            builder.MapPatch("/todos/{id:int}", UpdateTodo).WithTags(EndpointTags.Todos);
        }

        private static async Task<Results<NoContent, ValidationProblem, NotFound>> UpdateTodo(
            int id,
            UpdateTodoCommand request,
            IValidator<UpdateTodoCommand> validator,
            TodoContext context,
            CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
                return TypedResults.ValidationProblem(validationResult.ToDictionary());

            var todo = await context.Todos.FindAsync(id, cancellationToken);

            if (todo is null)
                return TypedResults.NotFound();

            todo.Update(request.Title, request.Description, request.Status);
            await context.SaveChangesAsync(cancellationToken);

            return TypedResults.NoContent();
        }
    }
}