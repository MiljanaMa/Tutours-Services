using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.Enums;
using FluentResults;

namespace Explorer.Tours.API.Public.TourAuthoring;

public interface IPublicEntityRequestService
{
    Result<PagedResult<PublicEntityRequestDto>> GetPaged(int page, int pageSize);
    Result<PublicEntityRequestDto> Get(int id);
    Result<PublicEntityRequestDto> CreateKeypointRequest(PublicEntityRequestDto publicEntityRequestDto);
    Result<PublicEntityRequestDto> CreateObjectRequest(PublicEntityRequestDto publicEntityRequestDto);
    Result Delete(int id);
    Result<PublicEntityRequestDto> Approve(PublicEntityRequestDto publicEntityRequestDto);
    Result<PublicEntityRequestDto> Decline(PublicEntityRequestDto publicEntityRequestDto);
    Result<PublicEntityRequestDto> GetByEntityId(int entityId, EntityType entityType);
}