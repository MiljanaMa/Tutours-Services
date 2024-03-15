using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public enum InvitationStatus
{
    PENDING,
    ACCEPTED,
    DENIED,
    CANCELLED
}

public class ClubInvitation : Entity
{
    public ClubInvitation()
    {
    }

    public ClubInvitation(long clubId, long touristId, InvitationStatus status)
    {
        ClubId = clubId;
        UserId = touristId;
        Status = status;
    }

    public long ClubId { get; init; }
    public long UserId { get; init; }
    public InvitationStatus Status { get; init; }
}