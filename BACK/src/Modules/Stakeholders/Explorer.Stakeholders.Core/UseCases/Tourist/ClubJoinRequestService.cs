using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases.Tourist;

public class ClubJoinRequestService : CrudService<ClubJoinRequestDto, ClubJoinRequest>, IClubJoinRequestService
{
    protected readonly IClubJoinRequestRepository _requestRepository;

    public ClubJoinRequestService(IClubJoinRequestRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _requestRepository = repository;
    }

    public Result<PagedResult<ClubJoinRequestDto>> GetAllByUser(int userId)
    {
        try
        {
            var requests = _requestRepository.GetAllByUser(userId);
            return MapToDto(requests);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result<PagedResult<ClubJoinRequestDto>> GetAllByClub(int clubId)
    {
        try
        {
            var requests = _requestRepository.GetAllByClub(clubId);
            return MapToDto(requests);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result<ClubJoinRequestDto> Create(ClubDto club, int userId)
    {
        // if (_requestRepository.Exists(club.Id, userId))
        //     return Result.Fail(FailureCode.Conflict).WithError("Request for this user already exists");
        // if (club.MemberIds.Any(id => id == userId))
        //     return Result.Fail(FailureCode.Conflict).WithError("User is member");
        // try
        // {
        //     var joinRequest = new ClubJoinRequestDto
        //     {
        //         Id = 0,
        //         UserId = userId,
        //         ClubId = club.Id,
        //         Status = ClubJoinRequestDto.JoinRequestStatus.Pending
        //     };
        //     var result = CrudRepository.Create(MapToDomain(joinRequest));
        //     return MapToDto(result);
        // }
        // catch (ArgumentException e)
        // {
        //     return Result.Fail(FailureCode.InvalidArgument).WithError($"Error while creating request: {e.Message}");
        // }

        throw new NotImplementedException();
    }
}