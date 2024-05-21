using System.Text;
using Newtonsoft.Json;

namespace RackApi.IntegrationTest;

public class UserIntegrationTests
{
    private HttpClient _client;
    private HttpClient _client2;
    private HttpClient _client3;
    private HttpClient _client4;

    [SetUp]
    public void Setup()
    {
        // Initialize HttpClient
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://api:80");
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
        var response = await _client.PostAsync("/User", content);

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [SetUp]
    public void SetupBasicUser()
    {
        // Initialize HttpClient
        _client2 = new HttpClient();
        _client2.BaseAddress = new Uri("http://user:5114");
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
    
    [SetUp]
    public void Setup2()
    {
        // Initialize HttpClient
        _client3 = new HttpClient();
        _client3.BaseAddress = new Uri("http://rackapi_api_1:80");
    }

    [Test]
    public async Task Test_User_Creation2()
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
        var response = await _client3.PostAsync("/User", content);

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [SetUp]
    public void SetupBasicUser2()
    {
        // Initialize HttpClient
        _client4 = new HttpClient();
        _client4.BaseAddress = new Uri("http://rackapi_user_1:5114");
    }

    [Test]
    public async Task Test_User_Creation_User_Level2()
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
        var response = await _client4.PostAsync(_client4.BaseAddress, content);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [TearDown]
    public void TearDown()
    {
        // Initialize HttpClient
        _client.Dispose();
        _client2.Dispose();
        _client3.Dispose();
        _client4.Dispose();
    }
}