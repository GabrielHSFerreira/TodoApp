using MediatR;

namespace TodoIntegrationTests.WebApi.Features.GetTodo
{
    public record GetTodoQuery : IRequest<GetTodoResponse?>
    {
        public int Id { get; init; }

        public GetTodoQuery(int id)
        {
            Id = id;
        }
    }
}