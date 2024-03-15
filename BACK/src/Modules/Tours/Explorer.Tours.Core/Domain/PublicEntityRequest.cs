using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Enum;

namespace Explorer.Tours.Core.Domain;

public class PublicEntityRequest : Entity
{
    public PublicEntityRequest()
    {
    }

    public PublicEntityRequest(int userId, int entityId, EntityType entityType, PublicEntityRequestStatus status,
        string? comment)
    {
        UserId = userId;
        EntityId = entityId;
        EntityType = entityType;
        Status = status;
        Comment = comment;
    }

    public int UserId { get; init; }
    public int EntityId { get; init; }
    public EntityType EntityType { get; init; }
    public PublicEntityRequestStatus Status { get; init; }
    public string? Comment { get; init; }
}