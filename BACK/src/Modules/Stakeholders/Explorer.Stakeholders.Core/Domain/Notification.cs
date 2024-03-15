using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Enums;

namespace Explorer.Stakeholders.Core.Domain;

public class Notification : Entity
{
    public Notification()
    {
    }

    public Notification(int userId, NotificationType type, string content, string actionURL, DateTime creationDateTime,
        bool isRead)
    {
        UserId = userId;
        Type = type;
        Content = content;
        ActionURL = actionURL;
        CreationDateTime = creationDateTime;
        IsRead = isRead;
    }

    public Notification(int userId, NotificationType type, string actionURL, DateTime creationDateTime, bool isRead,
        string additionalMessage)
    {
        UserId = userId;
        Type = type;
        GenerateContent(additionalMessage);
        ActionURL = actionURL;
        CreationDateTime = creationDateTime;
        IsRead = isRead;
    }

    public int UserId { get; init; }
    public NotificationType Type { get; init; }
    public string? Content { get; private set; }
    public string? ActionURL { get; init; }
    public DateTime CreationDateTime { get; init; }
    public bool IsRead { get; private set; }

    private void GenerateContent(string additionalMessage)
    {
        switch (Type)
        {
            case NotificationType.ISSUE_COMMENT:
                Content = "A new comment has been added to the tour issue for the tour named '" + additionalMessage +
                          "'.";
                break;
            case NotificationType.ISSUE_DEADLINE:
                Content = "The deadline for resolving the tour issue has been set to " + additionalMessage;
                break;
            case NotificationType.MESSAGE:
                Content = "You have received a new message from " + additionalMessage;
                break;
            case NotificationType.REQUEST_ACCEPTED:
                Content = "The request for public site registration '" + additionalMessage + "' has been accepted.";
                break;
            case NotificationType.REQUEST_DECLINED:
                Content = "The request for public site registration '" + additionalMessage + "' has been declined.";
                break;
            case NotificationType.COINS_GIFTED:
                Content = "Great news! You've just been gifted some coins!";
                break;
            case NotificationType.TOUR_PURCHASED:
                Content = "The purchase was successful. You can see the purchased tour in your collection.";
                break;
            default:
                throw new Exception("Invalid Notification Type");
        }
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}