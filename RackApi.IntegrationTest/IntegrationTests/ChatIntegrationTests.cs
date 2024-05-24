using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace RackApi.IntegrationTest;

public class ChatIntegrationTests
{
    private HttpClient _client;
    private string _jwtToken;

    [SetUp]
    public void Setup()
    {
        // Initialize HttpClient
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://localhost:5283/Message");
        
        _jwtToken = JwtHelper.GenerateToken(9);
    }
    
    [Test]
    public async Task Test_Chat_Creation()
    {
        // Arrange
        var chat = new
        {
            UserId = 9, 
            Message = "Hello!", 
            CreatedAt = DateTime.Now, 
            ReadStatus = 0, 
            ToUserId = 2
        };
        var content = new StringContent(JsonConvert.SerializeObject(chat), Encoding.UTF8, "application/json");

        // Add the authentication token to the request headers
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

        // Act
        var response = await _client.PostAsync(_client.BaseAddress, content);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [TearDown]
    public void TearDown()
    {
        // Initialize HttpClient
        _client.Dispose();
    }
}