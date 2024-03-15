namespace Explorer.Stakeholders.API.Dtos;

public class TourIssueDto
{
    public int Id { get; set; }
    public required string Category { get; set; }
    public uint Priority { get; set; }
    public required string Description { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime? ResolveDateTime { get; set; }
    public bool IsResolved { get; set; }
    public int TourId { get; set; }
    public List<TourIssueCommentDto> Comments { get; set; }
    public int UserId { get; set; }
}