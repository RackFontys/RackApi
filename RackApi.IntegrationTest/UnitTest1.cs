using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace RackApi.IntegrationTest;

public class IntegrationTests
{
    private readonly HttpClient _client;
    private readonly TestServer _server;

    public IntegrationTests()
    {
        // Arrange
        _server = new TestServer(new WebHostBuilder().UseStartup<Program>());
        _client = _server.CreateClient();
    }

    [Fact]
    public async Task ReturnHelloWorld()
    {
        // Act
        var response = await _client.GetAsync(":5012/User?email=test&password=test");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        // Assert
        Assert.That(responseString, Is.SameAs("Hello world!"));
    }
}