using System.Text;
using Newtonsoft.Json;

namespace RackApi.IntegrationTest;

public class UserIntegrationTests
{
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        // Initialize HttpClient
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://localhost:5283/User");
    }

    [Test]
    public async Task Test_User_Creation()
    {
        // Arrange
        var user = new 
        { 
            Id = 0,
            Name = "Test User", 
            Email = "test@example.com", 
            CreatedAt = DateTime.UtcNow,
            CompanyId = 1,
            Password = "testpassword"
        };
        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(_client.BaseAddress, content);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }
}