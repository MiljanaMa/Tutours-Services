using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Dtos.Enums;
using FluentResults;

namespace Explorer.Stakeholders.API.Public.Tourist;

public interface IClubService
{
    Result<PagedResult<ClubDto>> GetPaged(int page, int pageSize);
    Result<PagedResult<ClubDto>> GetAll(int page, int pageSize);
    Result<ClubDto> Create(ClubDto club);
    Result<ClubDto> Update(ClubDto club);
    Result<ClubDto> Get(int id);
    Result<ClubDto> GetWithMembers(int id);
    Result<ClubDto> GetUntracked(long id);

    Result Delete(int id);
    PagedResult<ClubDto> GetAllByUser(int page, int pageSize, int userId);
    Result<ClubDto> AddAchievement(long clubId, long achievementId);
}