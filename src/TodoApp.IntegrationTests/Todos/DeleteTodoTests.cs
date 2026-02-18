using System.Net;
using TodoApp.IntegrationTests.Fixtures;

namespace TodoApp.IntegrationTests.Todos
{
    public class DeleteTodoTests
    {
        private readonly WebApiFactory _factory;

        public DeleteTodoTests(PostgreSqlDatabase database)
        {
            _factory = new WebApiFactory(database);
        }

        [Fact]
        public async Task DeleteTodo_Inexistent_NoContentResponse()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync("/todos/150000", TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTodo_Existent_NoContentResponse()
        {
            // Arrange
            var createdTodo = await _factory.Context.CreateTodo(TestContext.Current.CancellationToken);
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"/todos/{createdTodo.Id}", TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var isExistentTodo = await _factory.Context.TodoExists(createdTodo.Id, TestContext.Current.CancellationToken);
            Assert.False(isExistentTodo, "Entity was not deleted.");
        }
    }
}