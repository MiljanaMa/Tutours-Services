using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Internal
{
    public interface IInternalTouristPositionService
    {
        Result<TouristPositionDto> GetByUser(long userId);
        Result<PagedResult<TouristPositionDto>> GetPaged(int page, int pageSize);
    }
}
