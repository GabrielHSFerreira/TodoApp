using FluentValidation;

namespace TodoApp.WebApi.Features.UpdateTodo
{
    public class UpdateTodoValidator : AbstractValidator<UpdateTodoCommand>
    {
        public UpdateTodoValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotNull();
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}