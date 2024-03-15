using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Enums;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class ClubChallengeRequestService : BaseService<ClubChallengeRequestDto, ClubChallengeRequest>, IClubChallengeRequestService
{
    private readonly IClubChallengeRequestRepository _challengeRequestRepository;
    private readonly IClubService _clubService;
    private readonly IClubFightService _fightService;

    public ClubChallengeRequestService(IClubChallengeRequestRepository repository, IClubService clubService,
       IClubFightService fightService, IMapper mapper): base(mapper)
    {
        _challengeRequestRepository = repository;
        _clubService = clubService;
        _fightService = fightService;
    }

    public Result<ClubChallengeRequestDto> Create(ClubChallengeRequestDto request)
    {
        request.Status = "PENDING";
        request.Challenger = null;
        request.Challenged = null;
        var result = _challengeRequestRepository.Create(MapToDomain(request));
        return MapToDto(result);
    }

    public Result<ClubChallengeRequestDto> Update(ClubChallengeRequestDto request)
    {
        var result = _challengeRequestRepository.Update(MapToDomain(request));
        return MapToDto(result);
    }

    public Result<ClubChallengeRequestDto> AcceptChallenge(ClubChallengeRequestDto request)
    {
        ClubChallengeRequest existingRequest = _challengeRequestRepository.GetNoTracking((long)request.Id);
        if (existingRequest.Status == ClubChallengeRequestStatus.ACCEPTED)
        {
            return Result.Fail("Already accepted");
        }
        
        var fight = _fightService.CreateFromRequest(request);
        if (fight == null)
        {
            return Result.Fail("One of the clubs are currently in active fight");
        }

        request.Status = "ACCEPTED";
        var result = Update(request);
        return result;
    }

    public Result<List<ClubChallengeRequestDto>> GetCurrentChallengesForClub(long clubId)
    {
        var result = _challengeRequestRepository.GetCurrentChallengesForClub(clubId);
        return MapToDto(result);
    }
}