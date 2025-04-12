using BetterOutcome;
using MediatR;

namespace TodoIntegrationTests.WebApi.Features.GetTodo
{
    public record GetTodoQuery : IRequest<Option<GetTodoResponse>>
    {
        public int Id { get; init; }

        public GetTodoQuery(int id)
        {
            Id = id;
        }
    }
}