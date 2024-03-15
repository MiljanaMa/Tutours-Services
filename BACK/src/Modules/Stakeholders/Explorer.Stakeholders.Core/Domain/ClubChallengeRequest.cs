using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Enums;

namespace Explorer.Stakeholders.Core.Domain;

public class ClubChallengeRequest : Entity
{
    public Club? Challenger { get; init; }
    public long ChallengerId { get; init; }
    public Club? Challenged { get; init; }
    public long ChallengedId { get; init; }
    public ClubChallengeRequestStatus Status { get; init; }

    public ClubChallengeRequest(long challengerId, long challengedId, ClubChallengeRequestStatus status)
    {
        ChallengerId = challengerId;
        ChallengedId = challengedId;
        Status = status;
    }
}