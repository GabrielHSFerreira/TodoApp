using MediatR;
using TodoApp.WebApi.Contexts;
using TodoApp.WebApi.Domain;

namespace TodoApp.WebApi.Features.CreateTodo
{
    public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, CreateTodoResponse>
    {
        private readonly TodoContext _context;

        public CreateTodoHandler(TodoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CreateTodoResponse> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = new Todo(request.Title, request.Description);

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateTodoResponse(todo.Id);
        }
    }
}