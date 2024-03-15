using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.MarketPlace;

public interface ITourFilteringService
{
    public Result<PagedResult<TourDto>> GetFilteredTours(int page, int pageSize, FilterCriteriaDto filter);
}