using MediatR;
using System.ComponentModel.DataAnnotations;
using TodoIntegrationTests.WebApi.Domain;

namespace TodoIntegrationTests.WebApi.Features.UpdateTodo
{
    public record UpdateTodoCommand : IRequest<bool>
    {
        public int Id { get; init; }

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