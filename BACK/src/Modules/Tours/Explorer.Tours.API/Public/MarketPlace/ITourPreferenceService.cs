using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.MarketPlace;

public interface ITourPreferenceService
{
    Result<TourPreferenceDto> GetByUser(int userId);
    Result<TourPreferenceDto> Create(TourPreferenceDto tourPreference);
    Result<TourPreferenceDto> Update(TourPreferenceDto tourPreference);
    Result Delete(int userId);
}