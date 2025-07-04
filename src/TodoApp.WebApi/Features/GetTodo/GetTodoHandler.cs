using BetterOutcome;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.WebApi.Contexts;

namespace TodoApp.WebApi.Features.GetTodo
{
    public class GetTodoHandler : IRequestHandler<GetTodoQuery, Option<GetTodoResponse>>
    {
        private readonly TodoContext _context;

        public GetTodoHandler(TodoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Option<GetTodoResponse>> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            var todo = await _context.Todos
                .Where(x => x.Id == request.Id)
                .Select(x => new GetTodoResponse(
                    x.Id,
                    x.CreatedAt,
                    x.UpdatedAt,
                    x.Title,
                    x.Description,
                    x.Status))
                .FirstOrDefaultAsync(cancellationToken);

            return Option<GetTodoResponse>.CreateFromValue(todo);
        }
    }
}