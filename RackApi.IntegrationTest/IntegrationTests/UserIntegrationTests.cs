using System.Text;
using Newtonsoft.Json;

namespace RackApi.IntegrationTest;

public class UserIntegrationTests
{
    private HttpClient _client;
    private HttpClient _client2;

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
    
    [SetUp]
    public void SetupBasicUser()
    {
        // Initialize HttpClient
        _client2 = new HttpClient();
        _client2.BaseAddress = new Uri("http://localhost:5114");
    }

    [Test]
    public async Task Test_User_Creation_User_Level()
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
        var response = await _client2.PostAsync(_client2.BaseAddress, content);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [TearDown]
    public void TearDown()
    {
        // Initialize HttpClient
        _client.Dispose();
        _client2.Dispose();
    }
}