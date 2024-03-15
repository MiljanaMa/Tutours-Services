using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public.Tourist;

public interface IClubInvitationService
{
    Result<PagedResult<ClubInvitationDto>> GetPaged(int page, int pageSize);
    Result<ClubInvitationDto> Create(ClubInvitationDto invitation);
    Result<ClubInvitationDto> Update(ClubInvitationDto invitation);
}