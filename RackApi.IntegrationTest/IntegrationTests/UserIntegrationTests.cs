using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;

namespace RackApi.IntegrationTest;

public class UserIntegrationTests
{
    private HttpClient _client;
    private string _jwtToken;

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

    [Test]
    public async Task Test_User_Retrieval()
    {
        // Arrange
        

        // Act
        var response = await _client.GetAsync("?email=test@example.com&password=testpassword");

        string responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task Test_User_Delete()
    {
        // Arrange
        _jwtToken = JwtHelper.GenerateToken(1);

        // Act
        var response = await _client.DeleteAsync(_client.BaseAddress);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }
}