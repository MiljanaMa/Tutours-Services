using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.TourAuthoring;

public class KeypointService : CrudService<KeypointDto, Keypoint>, IKeypointService
{
    protected readonly IKeypointRepository _keypointRepository;

    public KeypointService(IKeypointRepository keypointRepository, IMapper mapper) : base(keypointRepository, mapper)
    {
        _keypointRepository = keypointRepository;
    }

    public Result<List<KeypointDto>> CreateMultiple(List<KeypointDto> keypoints)
    {
        var results = new List<KeypointDto>();
        foreach (var keypoint in keypoints)
        {
            var result = CrudRepository.Create(MapToDomain(keypoint));
            results.Add(MapToDto(result));
        }

        return results;
    }

    public Result<PagedResult<KeypointDto>> GetByTourId(int page, int pageSize, int tourId)
    {
        var result = _keypointRepository.GetByTour(page, pageSize, tourId);
        return MapToDto(result);
    }
}