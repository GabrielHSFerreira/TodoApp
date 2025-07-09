using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.WebApi.Features.UpdateTodo;

namespace TodoApp.WebApi.Controllers
{
    [Route("todos")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ISender _sender;

        public TodosController(ISender sender)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTodo(int id, UpdateTodoCommand command)
        {
            var result = await _sender.Send(command with { Id = id });

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}