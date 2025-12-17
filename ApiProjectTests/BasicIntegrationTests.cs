using Microsoft.AspNetCore.Mvc.Testing;

public class BasicIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BasicIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetEndpoints_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast"); // Use an existing endpoint

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
}