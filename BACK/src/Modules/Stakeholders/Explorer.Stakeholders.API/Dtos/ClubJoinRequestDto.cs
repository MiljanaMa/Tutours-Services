namespace Explorer.Stakeholders.API.Dtos;

public class ClubJoinRequestDto
{
    public enum JoinRequestStatus
    {
        Accepted,
        Rejected,
        Pending,
        Canceled
    }

    public long Id { get; set; }
    public long UserId { get; set; }
    public long ClubId { get; set; }
    public JoinRequestStatus Status { get; set; }
}