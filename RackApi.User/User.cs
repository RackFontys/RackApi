namespace RackApi.User;

public class User
{
    public int id { get; set; }

    public string name { get; set; }

    public string email { get; set; }

    public DateTime createdAt { get; set; }

    public int companyId { get; set; }
}