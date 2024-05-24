using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;

namespace RackApi.IntegrationTest;

public class UserIntegrationTests
{
    private HttpClient _client;
    private string _jwtToken;
    private int _userId;

    [SetUp]
    public void Setup()
    {
        // Initialize HttpClient
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://localhost:5283/User");
    }

    [Test, Order(1)]
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

    [Test, Order(2)]
    public async Task Test_User_Retrieval()
    {
        // Arrange
        

        // Act
        var response = await _client.GetAsync("?email=test@example.com&password=testpassword");

        _jwtToken = await response.Content.ReadAsStringAsync();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(_jwtToken);

        // Extract claims from the payload
        var claims = new Dictionary<string, object>();
        foreach (var claim in jwtSecurityToken.Claims)
        {
            claims.Add(claim.Type, claim.Value);
        }
        _userId = Convert.ToInt16(claims["userId"]);

        Console.WriteLine(_userId);
        Console.WriteLine(_jwtToken);
        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Test, Order(3)]
    public async Task Test_User_Delete()
    {
        // Arrange
        // _jwtToken = JwtHelper.GenerateToken(9);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        
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