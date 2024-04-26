using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RackApi.IntegrationTest;

public class ApiIntegrationTests
{
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        // Initialize HttpClient
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://localhost:5283");
    }

    [Test]
    public async Task Test_User_Creation()
    {
        // Arrange
        var user = new 
        { 
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
        // Additional assertions...
    }

    // [Test]
    // public async Task Test_Chat_Creation()
    // {
    //     // Arrange
    //     var chat = new { UserId = "user_id", Message = "Hello, world!" };
    //     var content = new StringContent(JsonConvert.SerializeObject(chat), Encoding.UTF8, "application/json");
    //
    //     // Act
    //     var response = await _client.PostAsync("/Message", content);
    //
    //     // Assert
    //     response.EnsureSuccessStatusCode();
    //     // Additional assertions...
    // }

    [TearDown]
    public void TearDown()
    {
        // Cleanup
        _client.Dispose();
    }
}