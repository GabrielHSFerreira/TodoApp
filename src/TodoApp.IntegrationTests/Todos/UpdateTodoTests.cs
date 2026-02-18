using System.Net;
using System.Net.Http.Json;
using TodoApp.IntegrationTests.Fixtures;
using TodoApp.WebApi.Domain;
using TodoApp.WebApi.Features.UpdateTodo;

namespace TodoApp.IntegrationTests.Todos
{
    public class UpdateTodoTests
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
            var oldTodo = await _factory.Context.CreateTodo(TestContext.Current.CancellationToken);
            var request = new UpdateTodoCommand("New title", "New description", TodoStatus.Done);

            // Act
            var response = await client.PatchAsJsonAsync(
                $"/todos/{oldTodo.Id}",
                request,
                TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var updatedTodo = await _factory.Context.FindTodo(oldTodo.Id, TestContext.Current.CancellationToken);
            Assert.NotNull(updatedTodo);
            Assert.Equal(request.Title, updatedTodo.Title);
            Assert.Equal(request.Description, updatedTodo.Description);
            Assert.Equal(request.Status, updatedTodo.Status);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "Test", TodoStatus.Done)]
        [InlineData("Test", null, TodoStatus.Pending)]
        [InlineData("Test", "Test", null)]
        [InlineData("Test", "Test", (TodoStatus)1000)]
        [InlineData("", "", (TodoStatus)1000)]
        public async Task UpdateTodo_InvalidRequest_BadRequestResponse(string? title, string? description, TodoStatus? status)
        {
            // Arrange
            var client = _factory.CreateClient();
            var oldTodo = await _factory.Context.CreateTodo(TestContext.Current.CancellationToken);

            // Act
            var response = await client.PatchAsJsonAsync(
                $"/todos/{oldTodo.Id}",
                new { Title = title, Description = description, Status = status },
                TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}