using System.Net;
using TodoApp.IntegrationTests.Fixtures;

namespace TodoApp.IntegrationTests.Todos
{
    public class DeleteTodoTests : IClassFixture<WebApiFactory>
    {
        private readonly WebApiFactory _factory;

        public DeleteTodoTests(WebApiFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task DeleteTodo_Inexistent_NoContentResponse()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync("/todos/150000");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTodo_Existent_NoContentResponse()
        {
            // Arrange
            var createdTodo = await _factory.Context.CreateTodo();
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"/todos/{createdTodo.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var isExistentTodo = await _factory.Context.TodoExists(createdTodo.Id);
            Assert.False(isExistentTodo, "Entity was not deleted.");
        }
    }
}