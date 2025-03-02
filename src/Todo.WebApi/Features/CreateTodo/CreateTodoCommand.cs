using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TodoIntegrationTests.WebApi.Features.CreateTodo
{
    public record CreateTodoCommand : IRequest<CreateTodoResponse>
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