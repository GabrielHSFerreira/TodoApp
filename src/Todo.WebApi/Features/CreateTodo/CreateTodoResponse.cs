namespace TodoIntegrationTests.WebApi.Features.CreateTodo
{
    public record CreateTodoResponse
    {
        public int CreatedId { get; init; }

        public CreateTodoResponse(int createdId)
        {
            CreatedId = createdId;
        }
    }
}