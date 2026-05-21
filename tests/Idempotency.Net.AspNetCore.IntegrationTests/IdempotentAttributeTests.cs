using System.Net;
using System.Net.Http.Json;

namespace Idempotency.Net.AspNetCore.IntegrationTests;

public class IdempotentAttributeTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;

    public IdempotentAttributeTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task TwoRequestsWithSameIdempotencyKey_ReturnSameResponse()
    {
        // Arrange
        var client = _factory.CreateClient();
        var key = Guid.NewGuid().ToString();

        var request1 = new HttpRequestMessage(HttpMethod.Post, "/orders")
        {
            Content = JsonContent.Create(new { item = "test" })
        };
        request1.Headers.Add("X-Idempotency-Key", key);

        var request2 = new HttpRequestMessage(HttpMethod.Post, "/orders")
        {
            Content = JsonContent.Create(new { item = "test" })
        };
        request2.Headers.Add("X-Idempotency-Key", key);

        // Act
        var response1 = await client.SendAsync(request1);
        var response2 = await client.SendAsync(request2);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

        var body1 = await response1.Content.ReadAsStringAsync();
        var body2 = await response2.Content.ReadAsStringAsync();

        Assert.Equal(body1, body2); 
    }

    [Fact]
    public async Task RequestsWithoutIdempotencyKey_AreIndependent()
    {
        // Arrange
        var client = _factory.CreateClient();

        var request1 = new HttpRequestMessage(HttpMethod.Post, "/orders")
        {
            Content = JsonContent.Create(new { item = "test" })
        };
        var request2 = new HttpRequestMessage(HttpMethod.Post, "/orders")
        {
            Content = JsonContent.Create(new { item = "test" })
        };

        // Act
        var response1 = await client.SendAsync(request1);
        var response2 = await client.SendAsync(request2);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

        var body1 = await response1.Content.ReadAsStringAsync();
        var body2 = await response2.Content.ReadAsStringAsync();

        Assert.NotEqual(body1, body2); 
    }
}