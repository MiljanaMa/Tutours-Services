using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class ClubJoinRequest : Entity
{
    public ClubJoinRequest(long userId, long clubId, JoinRequestStatus status)
    {
        UserId = userId;
        //User = user;
        ClubId = clubId;
        //Club = club;
        Status = status;
    }

    public long UserId { get; private set; }

    //public User User { get; private set; }
    public long ClubId { get; private set; }

    //club object
    public JoinRequestStatus Status { get; private set; }
}

public enum JoinRequestStatus
{
    Accepted,
    Rejected,
    Pending,
    Canceled
}