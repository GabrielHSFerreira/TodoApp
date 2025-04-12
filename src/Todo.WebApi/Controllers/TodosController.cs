using BetterOutcome;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoIntegrationTests.WebApi.Features.CreateTodo;
using TodoIntegrationTests.WebApi.Features.DeleteTodo;
using TodoIntegrationTests.WebApi.Features.GetTodo;
using TodoIntegrationTests.WebApi.Features.UpdateTodo;

namespace TodoIntegrationTests.WebApi.Controllers
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateTodoResponse))]
        public async Task<IActionResult> CreateTodo(CreateTodoCommand command)
        {
            var response = await _sender.Send(command);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTodoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTodo(int id)
        {
            var response = await _sender.Send(new GetTodoQuery(id));

            return response switch
            {
                Some<GetTodoResponse> todo => Ok(todo.Value),
                _ => NotFound()
            };
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            await _sender.Send(new DeleteTodoCommand(id));
            return NoContent();
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