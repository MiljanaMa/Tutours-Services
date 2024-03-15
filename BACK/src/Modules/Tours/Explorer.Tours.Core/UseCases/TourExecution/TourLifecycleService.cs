using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourExecution;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.UseCases.MarketPlace;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.TourExecution;

public class TourLifecycleService : BaseService<TourProgressDto, TourProgress>, ITourLifecycleService
{
    protected readonly IKeypointRepository _keypointRepository;
    protected readonly ITouristPositionRepository _touristPositionRepository;
    protected readonly ITourProgressRepository _tourProgressRepository;

    protected readonly ITourRepository _tourRepository;
    //protected readonly ITourPurchaseTokenRepository _tourPurchaseTokenRepository;

    public TourLifecycleService(ITourProgressRepository tourProgressRepository, ITourRepository tourRepository,
        ITouristPositionRepository touristPositionRepository,
        IKeypointRepository keypointRepository, /*ITourPurchaseTokenRepository tourPurchaseTokenRepository, */
        IMapper mapper) : base(mapper)
    {
        _tourRepository = tourRepository;
        _touristPositionRepository = touristPositionRepository;
        _tourProgressRepository = tourProgressRepository;
        _keypointRepository = keypointRepository;
        //_tourPurchaseTokenRepository = tourPurchaseTokenRepository;
    }

    public Result<TourProgressDto> GetActiveByUser(long userId)
    {
        try
        {
            var tourProgress = _tourProgressRepository.GetActiveByUser(userId);
            return MapToDto(tourProgress);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError("You don't have any started tours.");
        }
    }

    public Result<TourProgressDto> StartTour(long tourId, long userId)
    {
        // try to remove nested try-catch
        try
        {
            var touristPosition = _touristPositionRepository.GetByUser(userId);

            try
            {
                var existingTourProgress = _tourProgressRepository.GetActiveByUser(userId);
                return Result.Fail(FailureCode.NotFound).WithError("You already have tour that's in progress.");
            }
            catch (KeyNotFoundException e)
            {
                TourProgress tourProgress = new TourProgress(touristPosition.Id, tourId);
                _tourProgressRepository.Create(tourProgress);

                return MapToDto(tourProgress);
            }
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError("You can't start tour without setting your current location first.");
        }
    }

    public Result<TourProgressDto> AbandonActiveTour(long userId)
    {
        try
        {
            var tourProgress = _tourProgressRepository.GetActiveByUser(userId);

            tourProgress.Abandon();
            _tourProgressRepository.Update(tourProgress);

            return MapToDto(tourProgress);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError("You cannot abandon tour that is not in progress.");
        }
    }

    public Result<TourProgressDto> UpdateActiveTour(long userId, bool areRequiredEncountersDone)
    {
        try
        {
            var tourProgress = _tourProgressRepository.GetActiveByUser(userId);
            var touristPosition = tourProgress.TouristPosition;

            try
            {
                var currentKeypoint = _keypointRepository.GetByTourAndPosition(tourProgress.TourId, tourProgress.CurrentKeyPoint).FirstOrDefault();
                var dist = DistanceCalculator.CalculateDistance(touristPosition.Latitude, touristPosition.Longitude, currentKeypoint.Latitude, currentKeypoint.Longitude);
                
                if (tourProgress.CurrentKeyPoint == tourProgress.Tour.Keypoints.Count && areRequiredEncountersDone && dist <= 0.5)
                {
                    tourProgress.Complete();
                    touristPosition.UpdateTime();
                    tourProgress.UpdateActivityTime();
                    _tourProgressRepository.Update(tourProgress);
                    _touristPositionRepository.Update(touristPosition);
                    return MapToDto(tourProgress);

                }
                if (dist <= 0.5)
                {
                    var result = _keypointRepository.GetNextPositions(tourProgress.TourId, currentKeypoint.Position).ToList();
                    tourProgress.MoveToNextKeypoint(result[0] ?? 0);
                    touristPosition.UpdateTime();
                }

                tourProgress.UpdateActivityTime();
                _tourProgressRepository.Update(tourProgress);
                _touristPositionRepository.Update(touristPosition);
                return MapToDto(tourProgress);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError("Current keypoint not found.");
            }
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError("You cannot update tour that is not in progress.");
        }
    }
}