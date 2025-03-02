using MediatR;

namespace TodoIntegrationTests.WebApi.Features.DeleteTodo
{
    public record DeleteTodoCommand : IRequest
    {
        public int Id { get; init; }

        public DeleteTodoCommand(int id)
        {
            Id = id;
        }
    }
}