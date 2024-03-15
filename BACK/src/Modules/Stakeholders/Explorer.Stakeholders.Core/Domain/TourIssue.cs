using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class TourIssue : Entity
{
    public TourIssue()
    {
        Comments = new List<TourIssueComment>();
    }

    public TourIssue(string category, uint priority, string description, DateTime creationDateTime,
        DateTime? resolveDateTime, bool isResolved, int tourId, List<TourIssueComment>? comments, int userId)
    {
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentNullException("Category must be filled.");
        if (priority < 0 || priority > 5) throw new ArgumentException("Invalid priority.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentNullException("Description is required.");

        Category = category;
        Priority = priority;
        Description = description;
        CreationDateTime = creationDateTime;
        ResolveDateTime = resolveDateTime;
        IsResolved = isResolved;
        TourId = tourId;
        Comments = comments;
        UserId = userId;
    }

    public required string Category { get; init; }
    public uint Priority { get; init; }
    public required string Description { get; init; }
    public DateTime CreationDateTime { get; init; }
    public DateTime? ResolveDateTime { get; private set; }
    public bool IsResolved { get; private set; }
    public long TourId { get; init; }
    public ICollection<TourIssueComment> Comments { get; init; }
    public long UserId { get; init; }
}