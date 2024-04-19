using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RackApi.Chat.Models;

public class MessageModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UserId { get; set; }
    public int ToUserId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ReadStatus { get; set; }
}