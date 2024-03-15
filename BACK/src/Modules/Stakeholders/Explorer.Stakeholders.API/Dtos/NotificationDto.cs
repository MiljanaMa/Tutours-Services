namespace Explorer.Stakeholders.API.Dtos;

public class NotificationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Content { get; set; }
    public string? ActionURL { get; set; }
    public DateTime CreationDateTime { get; set; }
    public bool IsRead { get; set; }
}