using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases.Tourist;

public class ClubService : CrudService<ClubDto, Club>, IClubService, IInternalClubService
{
    private IClubRepository _clubRepository;
    private IAchievementRepository _achievementRepository;
    public ClubService(IClubRepository repository, IAchievementRepository achievementRepository,IMapper mapper) : base(repository, mapper)
    {
        _clubRepository = repository;
        _achievementRepository = achievementRepository;
    }

    public Result<ClubDto> GetWithMembers(int id)
    {
        var result =  _clubRepository.GetWithMembers(id);
        return MapToDto(result);
    }

    public Result<ClubDto> GetUntracked(long id)
    {
        var result =  _clubRepository.GetUntracked(id);
        return MapToDto(result);
    }
    public Result<PagedResult<ClubDto>> GetAll(int page, int pageSize)
    {
        var result = _clubRepository.GetAll(page, pageSize);
        return MapToDto(result);
    }

    public PagedResult<ClubDto> GetAllByUser(int page, int pageSize, int currentUserId)
    {
        // var result = GetPaged(page, pageSize);
        // var filteredItems = new List<ClubDto>();
        //
        // foreach (var c in result.ValueOrDefault.Results)
        //     if (c.OwnerId == currentUserId)
        //         filteredItems.Add(c);
        //
        // return new PagedResult<ClubDto>(filteredItems, filteredItems.Count);
        throw new NotImplementedException();
    }

    public Result<ClubDto> AddAchievement(long clubId, long achievementId)
    {
        Club club = _clubRepository.Get(clubId);
        club.Achievements.Add(_achievementRepository.Get(achievementId));
        var updatedClub = _clubRepository.Update(club);
        return MapToDto(updatedClub);
    }
}