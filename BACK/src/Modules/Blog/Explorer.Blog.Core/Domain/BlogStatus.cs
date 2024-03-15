using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Blog.Core.Domain;

public class BlogStatus : Entity
{
    public BlogStatus()
    {
    }

    public BlogStatus(long blogId, string name)
    {
        BlogId = blogId;
        Name = name;

        if (name == "") throw new ArgumentException("Name cannot be empty");
    }

    public long BlogId { get; init; }
    public string Name { get; init; }
}