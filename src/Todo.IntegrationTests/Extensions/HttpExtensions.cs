using System.Text.Json;

namespace TodoIntegrationTests.IntegrationTests.Extensions
{
    internal static class HttpExtensions
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<T> GetResponseBody<T>(this HttpResponseMessage response)
        {
            var body = await JsonSerializer.DeserializeAsync<T>(
                await response.Content.ReadAsStreamAsync(),
                _options);

            Assert.NotNull(body);

            return body;
        }
    }
}