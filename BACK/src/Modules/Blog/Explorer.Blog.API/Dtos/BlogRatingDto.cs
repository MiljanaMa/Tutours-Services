namespace Explorer.Blog.API.Dtos;

public class BlogRatingDto
{
    public long BlogId { get; set; }
    public long UserId { get; set; }
    public string Username { get; set; }
    public DateOnly CreationTime { get; set; }
    public string Rating { get; set; }
}