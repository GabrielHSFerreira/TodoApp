using System.Net;
using TodoApp.IntegrationTests.Extensions;
using TodoApp.IntegrationTests.Fixtures;
using TodoApp.WebApi.Features.GetTodo;

namespace TodoApp.IntegrationTests.Todos
{
    public class GetTodoTests : IClassFixture<WebApiFactory>
    {
        private readonly WebApiFactory _factory;

        public GetTodoTests(WebApiFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task GetTodo_Inexistent_NotFoundResponse()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/todos/150000", TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetTodo_Existent_FoundResponse()
        {
            // Arrange
            var createdTodo = await _factory.Context.CreateTodo(TestContext.Current.CancellationToken);
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/todos/{createdTodo.Id}", TestContext.Current.CancellationToken);

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
    }
}