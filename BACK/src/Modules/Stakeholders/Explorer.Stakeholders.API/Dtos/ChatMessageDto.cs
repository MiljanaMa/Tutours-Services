namespace Explorer.Stakeholders.API.Dtos;

public class ChatMessageDto
{
    public long Id { get; set; }
    public long SenderId { get; set; }
    public PersonDto Sender { get; set; }
    public long ReceiverId { get; set; }
    public PersonDto Receiver { get; set; }
    public string Content { get; set; }
    public DateTime CreationDateTime { get; set; }
    public bool IsRead { get; set; }
}