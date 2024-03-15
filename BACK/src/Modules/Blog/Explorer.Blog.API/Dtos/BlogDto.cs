namespace Explorer.Blog.API.Dtos;

public class BlogDto
{
    public int Id { get; set; }
    public int CreatorId { get; set; }
    public string? CreatorName { get; set; }
    public string? CreatorSurname { get; set; }
    public int CreatorRole { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string SystemStatus { get; set; }
    public DateOnly CreationDate { get; set; }
    public List<string>? ImageLinks { get; set; }
    public List<BlogStatusDto>? BlogStatuses { get; set; }
    public List<BlogRatingDto>? BlogRatings { get; set; }
}