using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.TourAuthoring;

public interface IObjectService
{
    Result<PagedResult<ObjectDto>> GetPaged(int page, int pageSize);
    Result<ObjectDto> Create(ObjectDto equipment);
    Result<ObjectDto> Update(ObjectDto equipment);
    Result Delete(int id);
    Result<PagedResult<ObjectDto>> GetPublicPagedInRange(int page, int pageSize, FilterCriteriaDto filter);
}