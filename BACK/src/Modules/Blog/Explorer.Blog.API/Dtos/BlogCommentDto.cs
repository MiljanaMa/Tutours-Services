namespace Explorer.Blog.API.Dtos;

public class BlogCommentDto
{
    public int Id { get; set; }
    public long BlogId { get; set; }
    public long UserId { get; set; }
    public string? Username { get; set; }
    public string? Comment { get; set; }
    public DateTime PostTime { get; set; }
    public DateTime LastEditTime { get; set; }
    public bool IsDeleted { get; set; }
}