using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.TourExecution;

public interface ITouristPositionService
{
    Result<TouristPositionDto> GetByUser(long userId);
    Result<TouristPositionDto> Create(TouristPositionDto touristPosition);
    Result<TouristPositionDto> Update(TouristPositionDto touristPosition);
}