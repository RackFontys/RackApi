namespace RackApi.Chat;

public class Message
{
    public DateOnly Date { get; set; }

    public string messageText { get; set; }

    public int id { get; set; }

    public int userId { get; set; }
}