using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.MarketPlace;

public class TourFilteringService : BaseService<TourDto, Tour>, ITourFilteringService
{
    protected readonly IObjectRepository _objectRepository;
    protected readonly IPublicKeypointRepository _publicKeypointRepository;
    protected readonly ITourRepository _tourRepository;

    public TourFilteringService(ITourRepository tourRepository, IPublicKeypointRepository publicKeypointRepository,
        IObjectRepository objectRepository, IMapper mapper) : base(mapper)
    {
        _tourRepository = tourRepository;
        _publicKeypointRepository = publicKeypointRepository;
        _objectRepository = objectRepository;
    }

    public Result<PagedResult<TourDto>> GetFilteredTours(int page, int pageSize, FilterCriteriaDto filter)
    {
        try
        {
            var nearbyTours = _tourRepository.GetPublishedPaged(0, 0).Results
                .Where(tour =>
                    tour.Keypoints.Any(keyPoint =>
                        DistanceCalculator.CalculateDistance(filter.CurrentLatitude, filter.CurrentLongitude,
                            keyPoint.Latitude, keyPoint.Longitude) <= filter.FilterRadius))
                .Select(tour => MapToDto(tour))
                .ToList();

            var totalTours = nearbyTours.Count();
            var pagedTours = nearbyTours;
            if (page != 0 && pageSize != 0)
            {
                pagedTours = nearbyTours.Skip((page - 1) * pageSize).ToList();
                pagedTours = pagedTours
                    .Take(pageSize)
                    .ToList();
            }

            var pagedResult = new PagedResult<TourDto>(pagedTours, totalTours);

            return Result.Ok(pagedResult);
        }
        catch (Exception e)
        {
            return Result.Fail<PagedResult<TourDto>>(e.Message);
        }
    }
}