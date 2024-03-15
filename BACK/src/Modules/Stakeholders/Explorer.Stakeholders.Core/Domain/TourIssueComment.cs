using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class TourIssueComment : Entity
{
    public TourIssueComment()
    {
    }

    public TourIssueComment(string comment, DateTime creationDateTime, int tourIssueId, int userId)
    {
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentNullException("Comment must not be empty!");
        Comment = comment;
        CreationDateTime = creationDateTime;
        TourIssueId = tourIssueId;
        UserId = userId;
    }

    public string Comment { get; init; }
    public DateTime CreationDateTime { get; init; }
    public long TourIssueId { get; init; }
    public long UserId { get; init; }
}