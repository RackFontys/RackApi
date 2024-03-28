using Microsoft.AspNetCore.Mvc;

namespace RackApi.User.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetUser")]
    public User Get()
    {
        return new User
        {
            id = Random.Shared.Next(0, 100),
            name = "Random Test user",
            email = "testuser@gmail.com",
            createdAt = DateTime.Now,
            companyId = Random.Shared.Next(0, 100)
        };
    }

    [HttpPost(Name = "UpdateUser")]
    public User Update(int id, string name, string email, int companyId)
    {
        return new User
        {
            id = id,
            name = name,
            email = email,
            companyId = companyId
        };
    }
}