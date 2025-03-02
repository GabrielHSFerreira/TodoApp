namespace TodoIntegrationTests.WebApi.Domain
{
    public class Todo
    {
        public int Id { get; init; }

        public DateTime CreatedAt { get; init; }

        public DateTime UpdatedAt { get; private set; }

        public string Title { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public TodoStatus Status { get; private set; }

        private Todo() { }

        public Todo(string title, string description)
        {
            var now = DateTime.UtcNow;
            CreatedAt = now;
            UpdatedAt = now;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public void Update(string title, string description, TodoStatus status)
        {
            UpdatedAt = DateTime.UtcNow;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Status = status;
        }
    }
}