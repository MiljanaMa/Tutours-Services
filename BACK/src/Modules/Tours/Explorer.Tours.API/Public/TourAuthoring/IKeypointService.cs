using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.TourAuthoring;

public interface IKeypointService
{
    Result<PagedResult<KeypointDto>> GetPaged(int page, int pageSize);
    Result<KeypointDto> Create(KeypointDto keypoint);
    Result<List<KeypointDto>> CreateMultiple(List<KeypointDto> keypoints);
    Result<KeypointDto> Update(KeypointDto keypoint);
    Result Delete(int id);
    Result<PagedResult<KeypointDto>> GetByTourId(int page, int pageSize, int tourId);
}