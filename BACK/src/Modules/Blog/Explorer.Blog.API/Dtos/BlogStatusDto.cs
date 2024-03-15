namespace Explorer.Blog.API.Dtos;

public class BlogStatusDto
{
    public long BlogId { get; set; }
    public int Id { get; set; }
    public required string Name { get; set; }
}