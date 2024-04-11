namespace RackApi.User.Models;

public class UserModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int CompanyId { get; set; }
    public string Password { get; set; }
}