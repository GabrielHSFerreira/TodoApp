using TodoApp.WebApi.Domain;

namespace TodoApp.WebApi.Features.GetTodo
{
    public record GetTodoResponse
    {
        public int Id { get; init; }

        public DateTime CreatedAt { get; init; }

        public DateTime UpdatedAt { get; init; }

        public string Title { get; init; }

        public string Description { get; init; }

        public TodoStatus Status { get; init; }

        public GetTodoResponse(
            int id,
            DateTime createdAt,
            DateTime updatedAt,
            string title,
            string description,
            TodoStatus status)
        {
            Id = id;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Status = status;
        }
    }
}