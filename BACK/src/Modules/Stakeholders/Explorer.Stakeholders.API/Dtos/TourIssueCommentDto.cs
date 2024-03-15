namespace Explorer.Stakeholders.API.Dtos;

public class TourIssueCommentDto
{
    public int Id { get; set; }
    public int TourIssueId { get; set; }
    public int UserId { get; set; }
    public required string Comment { get; set; }
    public DateTime CreationDateTime { get; set; }
}