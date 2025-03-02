using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoIntegrationTests.WebApi.Contexts;

namespace TodoIntegrationTests.WebApi.Features.GetTodo
{
    public class GetTodoHandler : IRequestHandler<GetTodoQuery, GetTodoResponse?>
    {
        private readonly TodoContext _context;

        public GetTodoHandler(TodoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<GetTodoResponse?> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            return _context.Todos
                .Where(x => x.Id == request.Id)
                .Select(x => new GetTodoResponse(
                    x.Id,
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.Title,
                    x.Description,
                    x.Status))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}