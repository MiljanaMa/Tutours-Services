using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.Enums;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using ObjectStatus = Explorer.Tours.Core.Domain.Enum.ObjectStatus;

namespace Explorer.Tours.Core.UseCases.TourAuthoring;

public class PublicEntityRequestService : CrudService<PublicEntityRequestDto, PublicEntityRequest>,
    IPublicEntityRequestService
{
    protected readonly IKeypointRepository _keypointRepository;
    private readonly IMapper _mapper;
    protected readonly IObjectRepository _objectRepository;
    protected readonly IPublicEntityRequestRepository _publicEntityRequestRepository;
    protected readonly IPublicKeypointRepository _publicKeypointRepository;

    public PublicEntityRequestService(IPublicEntityRequestRepository publicEntityRequestRepository,
        IKeypointRepository keypointRepository, IObjectRepository objectRepository,
        IPublicKeypointRepository publicKeypointRepository, IMapper mapper) : base(publicEntityRequestRepository,
        mapper)
    {
        _publicEntityRequestRepository = publicEntityRequestRepository;
        _keypointRepository = keypointRepository;
        _publicKeypointRepository = publicKeypointRepository;
        _objectRepository = objectRepository;
        _mapper = mapper;
    }

    public Result<PublicEntityRequestDto> CreateKeypointRequest(PublicEntityRequestDto publicEntityRequestDto)
    {
        try
        {
            var request = new PublicEntityRequestDto
            {
                UserId = publicEntityRequestDto.UserId,
                EntityId = publicEntityRequestDto.EntityId,
                EntityType = EntityType.KEYPOINT,
                Comment = publicEntityRequestDto.Comment,
                Status = PublicEntityRequestStatus.PENDING
            };
            var result = CrudRepository.Create(MapToDomain(request));
            return MapToDto(result);
        }
        catch (ArgumentException ex)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(ex.Message);
        }
    }

    public Result<PublicEntityRequestDto> CreateObjectRequest(PublicEntityRequestDto publicEntityRequestDto)
    {
        try
        {
            var request = new PublicEntityRequestDto
            {
                UserId = publicEntityRequestDto.UserId,
                EntityId = publicEntityRequestDto.EntityId,
                EntityType = EntityType.OBJECT,
                Comment = publicEntityRequestDto.Comment,
                Status = PublicEntityRequestStatus.PENDING
            };
            var result = CrudRepository.Create(MapToDomain(request));
            return MapToDto(result);
        }
        catch (ArgumentException ex)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(ex.Message);
        }
    }

    public Result<PublicEntityRequestDto> Approve(PublicEntityRequestDto publicEntityRequestDto)
    {
        if (publicEntityRequestDto == null) return Result.Fail("Request not found.");

        if (publicEntityRequestDto.Status is PublicEntityRequestStatus.APPROVED or PublicEntityRequestStatus.DECLINED)
            return Result.Fail("Request already approved or declined.");

        if (publicEntityRequestDto.Status is PublicEntityRequestStatus.PENDING)
        {
            if (publicEntityRequestDto.EntityType is EntityType.KEYPOINT)
            {
                var keypoint = _keypointRepository.Get(publicEntityRequestDto.EntityId);

                if (keypoint != null)
                {
                    var publicKeypoint = new PublicKeypoint
                    {
                        Name = keypoint.Name,
                        Latitude = keypoint.Latitude,
                        Longitude = keypoint.Longitude,
                        Description = keypoint.Description,
                        Image = keypoint.Image
                    };

                    _publicKeypointRepository.Create(publicKeypoint);
                }
                else
                {
                    return Result.Fail(FailureCode.NotFound).WithError("Keypoint is null.");
                }
            }
            else if (publicEntityRequestDto.EntityType is EntityType.OBJECT)
            {
                var tourObject = _objectRepository.Get(publicEntityRequestDto.EntityId);

                if (tourObject != null)
                {
                    tourObject.Status = ObjectStatus.PUBLIC;
                    _objectRepository.Update(tourObject);
                }
                else
                {
                    return Result.Fail(FailureCode.NotFound).WithError("Object is null.");
                }
            }

            publicEntityRequestDto.Status = PublicEntityRequestStatus.APPROVED;


            var request = MapToDomain(publicEntityRequestDto);
            _publicEntityRequestRepository.Update(request);

            return Result.Ok(_mapper.Map<PublicEntityRequestDto>(request));
        }

        return Result.Fail("Invalid request status.");
    }

    public Result<PublicEntityRequestDto> Decline(PublicEntityRequestDto publicEntityRequestDto)
    {
        if (publicEntityRequestDto == null) return Result.Fail("Request not found.");

        if (publicEntityRequestDto.Status is PublicEntityRequestStatus.PENDING)
        {
            publicEntityRequestDto.Status = PublicEntityRequestStatus.DECLINED;

            var request = MapToDomain(publicEntityRequestDto);
            _publicEntityRequestRepository.Update(request);

            return Result.Ok(publicEntityRequestDto);
        }

        if (publicEntityRequestDto.Status is PublicEntityRequestStatus.APPROVED or PublicEntityRequestStatus.DECLINED)
            return Result.Fail("Request already approved or declined.");
        return Result.Fail("Invalid request status.");
    }

    public Result<PublicEntityRequestDto> GetByEntityId(int entityId, EntityType entityType)
    {
        var coreEntityType = (Core.Domain.Enum.EntityType)Enum.Parse(typeof(Core.Domain.Enum.EntityType), entityType.ToString());
        var result = _publicEntityRequestRepository.GetByEntityId(entityId, coreEntityType);
        return MapToDto(result);
    }
}