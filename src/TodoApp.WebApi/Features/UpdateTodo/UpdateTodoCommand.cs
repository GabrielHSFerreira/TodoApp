using System.ComponentModel.DataAnnotations;
using TodoApp.WebApi.Domain;

namespace TodoApp.WebApi.Features.UpdateTodo
{
    public record UpdateTodoCommand
    {
        [Required]
        [MinLength(1)]
        public string Title { get; init; }

        public string Description { get; init; }

        [Required]
        [EnumDataType(typeof(TodoStatus))]
        public TodoStatus Status { get; init; }

        public UpdateTodoCommand(string title, string description, TodoStatus status)
        {
            Title = title;
            Description = description;
            Status = status;
        }
    }
}