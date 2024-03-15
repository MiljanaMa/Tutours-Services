using Explorer.Tours.API.Dtos.Enums;

namespace Explorer.Tours.API.Dtos;

public class PublicEntityRequestDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EntityId { get; set; }
    public EntityType EntityType { get; set; }
    public PublicEntityRequestStatus Status { get; set; }
    public string? Comment { get; init; }
}