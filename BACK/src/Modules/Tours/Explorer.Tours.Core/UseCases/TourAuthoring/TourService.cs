using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Internal;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.TourAuthoring;

public class TourService : CrudService<TourDto, Tour>, ITourService, IInternalTourService
{
    protected readonly ITourRepository _tourRepository;
    
    public TourService(ITourRepository tourRepository, IMapper mapper) : base(tourRepository, mapper)
    {
        _tourRepository = tourRepository;
    }

    public Result<PagedResult<TourDto>> GetByAuthor(int page, int pageSize, int id)
    {
        var result = _tourRepository.GetByAuthorPaged(page, pageSize, id);
        return MapToDto(result);
    }

    public Result<PagedResult<TourDto>> GetPublishedPaged(int page, int pageSize)
    {
        var result = _tourRepository.GetPublishedPaged(page, pageSize);
        return MapToDto(result);
    }

    public TourDto GetById(int id)
    {
        var tour = _tourRepository.Get(id);
        return MapToDto(tour);
    }

    public Result<PagedResult<TourDto>> GetArchivedAndPublishedPaged(int page, int pageSize)
    {
        var result = _tourRepository.GetArchivedAndPublishedPaged(page, pageSize);
        return MapToDto(result);
    }

    public Result<TourDto> CreateCustom(TourDto tour)
    {
        if (tour.Keypoints != null)
        {
            tour.Status = tour.Keypoints.Count < 2 || tour.Keypoints == null ? "DRAFT" : "CUSTOM";
        }
        else
        {
            tour.Status = "DRAFT";
        }

        var result = _tourRepository.Create(MapToDomain(tour));
        return MapToDto(result);
    }

    public Result<PagedResult<TourDto>> GetCustomByUserPaged(int userId, int page, int pageSize)
    {
        var result = _tourRepository.GetCustomByUserPaged(userId, page, pageSize);
        return MapToDto(result);
    }

    public Result<TourDto> CreateCampaign(TourDto tour)
    {
        tour.Status = "CAMPAIGN";
        var result = _tourRepository.Create(MapToDomain(tour));
        return MapToDto(result);
    }

    public Result<PagedResult<TourDto>> GetCampaignByUserPaged(int userId, int page, int pageSize)
    {
        var result = _tourRepository.GetCampaignByUserPaged(userId, page, pageSize);
        return MapToDto(result);
    }

    public Result<PagedResult<TourDto>> GetPublishedByAuthor(int authorId, int page, int pageSize)
    {
        var result = _tourRepository.GetPublishedByAuthor(authorId, page, pageSize);
        return MapToDto(result);
    }
}