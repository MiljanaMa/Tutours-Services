using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Internal;

public interface IInternalTourService
{
    Result<TourDto> Get(int id);
    Result<PagedResult<TourDto>> GetPaged(int page, int pageSize);
}