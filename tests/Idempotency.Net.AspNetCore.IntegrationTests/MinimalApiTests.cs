using System.Net;

namespace Idempotency.Net.AspNetCore.IntegrationTests;

public class MinimalApiTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;

    public MinimalApiTests(TestWebApplicationFactory factory) => _factory = factory;

    [Fact]
    public async Task TwoRequestsWithSameIdempotencyKey_ReturnSameResponse()
    {
        var client = _factory.CreateClient();
        var key = Guid.NewGuid().ToString();

        var request1 = new HttpRequestMessage(HttpMethod.Post, "/minimal-api");
        request1.Headers.Add("X-Idempotency-Key", key);
        var request2 = new HttpRequestMessage(HttpMethod.Post, "/minimal-api");
        request2.Headers.Add("X-Idempotency-Key", key);

        var resp1 = await client.SendAsync(request1);
        var resp2 = await client.SendAsync(request2);

        Assert.Equal(HttpStatusCode.OK, resp1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, resp2.StatusCode);
        Assert.Equal(await resp1.Content.ReadAsStringAsync(), await resp2.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task RequestsWithoutIdempotencyKey_AreIndependent()
    {
        var client = _factory.CreateClient();
        var resp1 = await client.PostAsync("/minimal-api", null);
        var resp2 = await client.PostAsync("/minimal-api", null);

        Assert.Equal(HttpStatusCode.OK, resp1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, resp2.StatusCode);
        Assert.NotEqual(await resp1.Content.ReadAsStringAsync(), await resp2.Content.ReadAsStringAsync());
    }
}