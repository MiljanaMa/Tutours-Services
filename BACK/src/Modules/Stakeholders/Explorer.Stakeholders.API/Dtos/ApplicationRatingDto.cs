namespace Explorer.Stakeholders.API.Dtos;

public class ApplicationRatingDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public int UserId { get; set; }
    public DateTime LastModified { get; set; }
}