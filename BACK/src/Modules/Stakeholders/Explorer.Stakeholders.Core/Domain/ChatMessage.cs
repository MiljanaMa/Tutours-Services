using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class ChatMessage : Entity
{
    public ChatMessage(long senderId, long receiverId, string content, DateTime creationDateTime, bool isRead)
    {
        SenderId = senderId;
        ReceiverId = receiverId;
        Content = content;
        CreationDateTime = creationDateTime;
        IsRead = isRead;
    }

    public long SenderId { get; set; }
    public Person Sender { get; set; }
    public long ReceiverId { get; set; }
    public Person Receiver { get; set; }
    public string Content { get; set; }
    public DateTime CreationDateTime { get; set; }
    public bool IsRead { get; private set; }

    public void MarkAsRead()
    {
        if (IsRead) throw new ArgumentException("Message is already read");

        IsRead = true;
    }
}