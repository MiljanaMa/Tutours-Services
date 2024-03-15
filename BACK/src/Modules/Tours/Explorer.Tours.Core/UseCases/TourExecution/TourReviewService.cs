using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourExecution;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.TourExecution;

public class TourReviewService : CrudService<TourReviewDto, TourReview>, ITourReviewService
{
    private readonly ITourProgressRepository _tourProgressRepository;
    protected readonly ITourReviewRepository _tourReviewRepository;
    private ICrudRepository<TourReview> _crudRepository;

    public TourReviewService(ICrudRepository<TourReview> crudRepository, ITourProgressRepository tourProgressRepository,
        ITourReviewRepository tourReviewRepository, IMapper mapper) : base(crudRepository, mapper)
    {
        _crudRepository = crudRepository;
        _tourProgressRepository = tourProgressRepository;
        _tourReviewRepository = tourReviewRepository;
    }

    public override Result<TourReviewDto> Create(TourReviewDto review)
    {
        try
        {
            var progress = _tourProgressRepository.GetByUser((long)review.UserId);
            var timeSpan = DateTime.UtcNow - progress.LastActivity;
            if (progress != null && progress.CurrentKeyPoint / (double)progress.Tour.Keypoints.Count > 0.35 &&
                timeSpan.Days < 7)
            {
                var result = CrudRepository.Create(MapToDomain(review));
                return MapToDto(result);
            }

            throw new ArgumentException("Tourist is not in tour or did not pass 35%!");
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public Result<PagedResult<TourReviewDto>> GetByTourId(long tourId, int page, int pageSize)
    {
        var result = _tourReviewRepository.GetByTourId(tourId, page, pageSize);
        return MapToDto(result);
    }

    public Result<double> CalculateAverageRate(List<TourReviewDto> tourReviews)
    {
        if (tourReviews == null || !tourReviews.Any()) return Result.Fail<double>("There are no tour reviews!");

        var averageRate = tourReviews.Average(r => r.Rating);

        return Result.Ok(averageRate);
    }
}