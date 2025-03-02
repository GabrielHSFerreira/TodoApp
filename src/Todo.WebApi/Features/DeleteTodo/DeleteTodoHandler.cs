using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoIntegrationTests.WebApi.Contexts;

namespace TodoIntegrationTests.WebApi.Features.DeleteTodo
{
    public class DeleteTodoHandler : IRequestHandler<DeleteTodoCommand>
    {
        private readonly TodoContext _context;

        public DeleteTodoHandler(TodoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            return _context.Todos
                .Where(todo => todo.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}