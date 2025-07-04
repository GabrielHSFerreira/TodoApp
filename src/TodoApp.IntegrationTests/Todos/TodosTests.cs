using Microsoft.EntityFrameworkCore;
using System.Net;
using TodoApp.IntegrationTests.Extensions;
using TodoApp.IntegrationTests.Fixtures;
using TodoApp.WebApi.Domain;
using TodoApp.WebApi.Features.GetTodo;

namespace TodoApp.IntegrationTests.Todos
{
    public class TodosTests : IClassFixture<WebApiFactory>
    {
        private readonly WebApiFactory _factory;

        public TodosTests(WebApiFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task GetTodo_Inexistent_NotFoundResponse()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/todos/150000");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetTodo_Existent_FoundResponse()
        {
            // Arrange
            var createdTodo = await CreateTodo();
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/todos/{createdTodo.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseTodo = await response.GetResponseBody<GetTodoResponse>();
            var areFieldsEqual =
                createdTodo.Id == responseTodo.Id
                && createdTodo.Title == responseTodo.Title
                && createdTodo.Description == responseTodo.Description
                && createdTodo.Status == responseTodo.Status;
            Assert.True(areFieldsEqual, "Entity and response fields are not equal.");
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
            var createdTodo = await CreateTodo();
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"/todos/{createdTodo.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var isExistentTodo = await _factory.Context.Todos.AnyAsync(t => t.Id == createdTodo.Id);
            Assert.False(isExistentTodo, "Entity was not deleted.");
        }

        private async Task<Todo> CreateTodo()
        {
            var context = _factory.Context;

            var newTodo = new Todo("Test", "Test");
            context.Todos.Add(newTodo);
            await context.SaveChangesAsync();

            return newTodo;
        }
    }
}