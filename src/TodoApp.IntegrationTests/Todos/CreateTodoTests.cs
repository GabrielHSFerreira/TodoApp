using System.Net;
using System.Net.Http.Json;
using TodoApp.IntegrationTests.Extensions;
using TodoApp.IntegrationTests.Fixtures;
using TodoApp.WebApi.Features.CreateTodo;

namespace TodoApp.IntegrationTests.Todos
{
    public class CreateTodoTests : IClassFixture<WebApiFactory>
    {
        private readonly WebApiFactory _factory;

        public CreateTodoTests(WebApiFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task CreateTodo_ValidRequest_OkResponse()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new CreateTodoCommand("Test title", "Test description");

            // Act
            var response = await client.PostAsJsonAsync("/todos", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.GetResponseBody<CreateTodoResponse>();
            Assert.NotEqual(0, body.CreatedId);
            var isExistentTodo = await _factory.Context.TodoExists(body.CreatedId);
            Assert.True(isExistentTodo);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "Test")]
        [InlineData("Test", null)]
        [InlineData("", "")]
        public async Task CreateTodo_InvalidRequest_BadRequestResponse(string? title, string? description)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/todos", new { Title = title, Description = description });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}