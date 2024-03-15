using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class ClubFightService : CrudService<ClubFightDto, ClubFight>, IClubFightService, IInternalClubFightService
{
    private readonly IClubFightRepository _fightRepository;
    
    public ClubFightService(IClubFightRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _fightRepository = repository;
    }

    public Result<ClubFightDto> GetWithClubs(int fightId)
    {
        var result = _fightRepository.GetWithClubs(fightId);

        return MapToDto(result);
    }

    public Result<ClubFightDto> Get(int fightId)
    {
        var result = _fightRepository.Get(fightId);

        return MapToDto(result);
    }

    public Result<List<ClubFightDto>> GetTricky()
    {
        var result = _fightRepository.GetTricky();

        return MapToDto(result);
    }
    
    

    public Result<ClubFightDto> CreateFromRequest(ClubChallengeRequestDto request)
    {
        ClubFight existingFight =
            _fightRepository.GetCurrentFightForOneOfTwoClubs(request.ChallengerId, request.ChallengedId);
        if (existingFight != null)
        {
            return null;
        }

        ClubFight newClubFight = new ClubFight(null, DateTime.UtcNow, request.ChallengerId, request.ChallengedId, true);
        var result = _fightRepository.Create(newClubFight);
        return MapToDto(result);
    }

    public Result<List<ClubFightDto>> GetPassedUnfinishedFights()
    {
        var result = _fightRepository.GetPassedUnfinishedFights();
        return MapToDto(result);
    }

    public Result<List<ClubFightDto>> UpdateMultiple(List<ClubFightDto> clubFights)
    {
        var results = new List<ClubFightDto>();
        foreach (var fight in clubFights)
        {
            var result = CrudRepository.Update(MapToDomain(fight));
            results.Add(MapToDto(result));
        }

        return results;
    }

    public Result<List<ClubFightDto>> GetAllByClub(int clubId)
    {
        var result = _fightRepository.GetAllByClub(clubId);

        return MapToDto(result);
    }
}