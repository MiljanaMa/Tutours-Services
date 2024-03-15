using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Blog.Core.Domain;

public class BlogComment : Entity
{
    public BlogComment()
    {
    }

    public BlogComment(int blogId, long userId, string comment, DateTime postTime, DateTime lastEditTime,
        bool isDeleted)
    {
        BlogId = blogId;
        UserId = userId;
        if (string.IsNullOrWhiteSpace(comment)) throw new ArgumentException("Invalid comment");

        Comment = comment;
        PostTime = postTime;
        LastEditTime = lastEditTime;
        IsDeleted = isDeleted;
    }

    public int BlogId { get; private set; }
    public long UserId { get; private set; }
    public string Comment { get; private set; }
    public DateTime PostTime { get; private set; }
    public DateTime LastEditTime { get; private set; }
    public bool IsDeleted { get; private set; }
}