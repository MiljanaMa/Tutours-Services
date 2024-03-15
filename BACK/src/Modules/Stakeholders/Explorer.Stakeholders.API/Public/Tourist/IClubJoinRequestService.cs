using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public.Tourist;

public interface IClubJoinRequestService
{
    Result<PagedResult<ClubJoinRequestDto>> GetPaged(int page, int pageSize);
    Result<PagedResult<ClubJoinRequestDto>> GetAllByUser(int userId);
    Result<PagedResult<ClubJoinRequestDto>> GetAllByClub(int clubId);
    Result<ClubJoinRequestDto> Create(ClubDto club, int userId);
    Result<ClubJoinRequestDto> Update(ClubJoinRequestDto request);
}