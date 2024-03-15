using Explorer.Blog.Core.Domain.Enum;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Blog.Core.Domain;

public class Blog : Entity
{
    public Blog()
    {
    }

    public Blog(string title, string description, DateOnly creationDate, List<string> imageLinks,
        BlogSystemStatus systemStatus, List<BlogStatus> blogStatuses, List<BlogRating> blogRatings)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.");

        Title = title;
        Description = description;
        CreationDate = creationDate;
        ImageLinks = imageLinks;
        SystemStatus = systemStatus;
        BlogStatuses = blogStatuses;
        BlogRatings = blogRatings;
    }

    public int CreatorId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public BlogSystemStatus SystemStatus { get; set; }
    public DateOnly CreationDate { get; init; }
    public List<string>? ImageLinks { get; init; }
    public ICollection<BlogStatus>? BlogStatuses { get; init; }
    public List<BlogRating>? BlogRatings { get; set; }

    public void AddRating(BlogRating blogRating)
    {
        if (BlogRatings == null) 
        {
            BlogRatings = new();
        }

        var foundRating =
            BlogRatings.FirstOrDefault(r => r.UserId == blogRating.UserId && r.BlogId == blogRating.BlogId);
        if (foundRating != null)
        {
            BlogRatings.RemoveAt(BlogRatings.IndexOf(foundRating));
            BlogRatings.Add(blogRating);
        }
        else
        {
            BlogRatings.Add(blogRating);
        }
    }

    public void CloseBlog()
    {
        SystemStatus = BlogSystemStatus.CLOSED;
    }
}