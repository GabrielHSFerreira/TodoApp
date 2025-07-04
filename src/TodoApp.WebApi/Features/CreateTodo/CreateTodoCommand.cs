using System.ComponentModel.DataAnnotations;

namespace TodoApp.WebApi.Features.CreateTodo
{
    public record CreateTodoCommand
    {
        [Required]
        [MinLength(1)]
        public string Title { get; init; }

        public string Description { get; init; }

        public CreateTodoCommand(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}