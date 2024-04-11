namespace RackApi.Chat.Models;

public class MessageModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ToUserId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ReadStatus { get; set; }
}