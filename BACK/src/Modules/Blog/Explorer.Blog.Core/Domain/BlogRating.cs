using System.Text.Json.Serialization;
using Explorer.Blog.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Blog.Core.Domain;

public class BlogRating : ValueObject<BlogRating>
{
    public BlogRating()
    {
    }

    [JsonConstructor]
    public BlogRating(long blogId, long userId, DateOnly creationTime, Rating rating)
    {
        BlogId = blogId;
        UserId = userId;
        Rating = rating;
        CreationTime = creationTime;
    }

    public long BlogId { get; }
    public long UserId { get; }
    public Rating Rating { get; private set; }
    public DateOnly CreationTime { get; }

    protected override bool EqualsCore(BlogRating rating)
    {
        return BlogId == rating.BlogId &&
               UserId == rating.UserId &&
               CreationTime == rating.CreationTime;
    }

    protected override int GetHashCodeCore()
    {
        throw new NotImplementedException();
    }
}