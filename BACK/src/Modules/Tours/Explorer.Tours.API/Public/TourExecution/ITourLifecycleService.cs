using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.TourExecution;

public interface ITourLifecycleService
{
    Result<TourProgressDto> GetActiveByUser(long userId);
    Result<TourProgressDto> StartTour(long tourId, long userId);
    Result<TourProgressDto> AbandonActiveTour(long userId);
    Result<TourProgressDto> UpdateActiveTour(long userId, bool areRequiredEncountersDone);
}