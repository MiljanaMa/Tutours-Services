namespace Explorer.Stakeholders.API.Dtos;

public enum InvitationStatus
{
    PENDING,
    ACCEPTED,
    DENIED,
    CANCELLED
}

public class ClubInvitationDto
{
    public long Id { get; set; }
    public long ClubId { get; set; }
    public long UserId { get; set; }
    public InvitationStatus Status { get; set; }
}