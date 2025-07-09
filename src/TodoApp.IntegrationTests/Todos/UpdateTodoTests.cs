using System.Net;
using System.Net.Http.Json;
using TodoApp.IntegrationTests.Fixtures;
using TodoApp.WebApi.Domain;
using TodoApp.WebApi.Features.UpdateTodo;

namespace TodoApp.IntegrationTests.Todos
{
    public class UpdateTodoTests : IClassFixture<WebApiFactory>
    {
        private readonly WebApiFactory _factory;

        public UpdateTodoTests(WebApiFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task UpdateTodo_ValidRequest_NoContentResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            var oldTodo = await _factory.Context.CreateTodo();
            var request = new UpdateTodoCommand("New title", "New description", TodoStatus.Done);

            // Act
            var response = await client.PatchAsJsonAsync($"/todos/{oldTodo.Id}", request);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var newTodo = await _factory.Context.Todos.FindAsync(oldTodo.Id);
            Assert.NotNull(newTodo);
            Assert.Equal(request.Title, newTodo.Title);
            Assert.Equal(request.Description, newTodo.Description);
            Assert.Equal(request.Status, newTodo.Status);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "Test", TodoStatus.Done)]
        [InlineData("Test", null, TodoStatus.Pending)]
        [InlineData("Test", "Test", null)]
        [InlineData("", "", TodoStatus.Blocked)]
        public async Task CreateTodo_InvalidRequest_BadRequestResponse(string? title, string? description, TodoStatus? status)
        {
            // Arrange
            var client = _factory.CreateClient();
            var oldTodo = await _factory.Context.CreateTodo();

            // Act
            var response = await client.PatchAsJsonAsync($"/todos/{oldTodo.Id}", new { Title = title, Description = description, Status = status });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}