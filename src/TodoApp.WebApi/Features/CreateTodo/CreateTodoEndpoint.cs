using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using TodoApp.WebApi.Contexts;
using TodoApp.WebApi.Domain;

namespace TodoApp.WebApi.Features.CreateTodo
{
    public class CreateTodoEndpoint : IEndpoint
    {
        public void Register(IEndpointRouteBuilder builder)
        {
            builder.MapPost("/todos", CreateTodo).WithTags(EndpointTags.Todos);
        }

        private static async Task<Results<Ok<CreateTodoResponse>, ValidationProblem>> CreateTodo(
            CreateTodoCommand request,
            IValidator<CreateTodoCommand> validator,
            TodoContext context,
            CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
                return TypedResults.ValidationProblem(validationResult.ToDictionary());

            var todo = new Todo(request.Title, request.Description);

            context.Todos.Add(todo);
            await context.SaveChangesAsync(cancellationToken);

            return TypedResults.Ok(new CreateTodoResponse(todo.Id));
        }
    }
}