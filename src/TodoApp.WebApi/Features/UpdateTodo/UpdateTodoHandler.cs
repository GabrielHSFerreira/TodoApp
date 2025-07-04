using MediatR;
using TodoApp.WebApi.Contexts;

namespace TodoApp.WebApi.Features.UpdateTodo
{
    public class UpdateTodoHandler : IRequestHandler<UpdateTodoCommand, bool>
    {
        private readonly TodoContext _context;

        public UpdateTodoHandler(TodoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _context.Todos.FindAsync(request.Id, cancellationToken);

            if (todo is null)
                return false;

            todo.Update(request.Title, request.Description, request.Status);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}