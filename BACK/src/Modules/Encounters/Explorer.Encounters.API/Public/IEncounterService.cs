using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using FluentResults;

namespace Explorer.Encounters.API.Public
{
    public interface IEncounterService
    {
        Result <PagedResult<EncounterDto>> GetPaged (int page, int pageSize);
        Result<EncounterDto> Get(int id);
        Result<EncounterDto> Create(EncounterDto encounter);
        Result<EncounterDto> Update(EncounterDto encounter);
        Result Delete(int id);
        Result<PagedResult<EncounterDto>> GetApproved(int page, int pageSize);
        Result<PagedResult<EncounterDto>> GetApprovedByStatus(int page, int pageSize, string status);
        Result<PagedResult<EncounterDto>> GetByUser(int page, int pageSize, long userId);
        Result<EncounterDto> Approve(EncounterDto encounter);
        Result<EncounterDto> Decline(EncounterDto encounter);
        Result<PagedResult<EncounterDto>> GetTouristCreatedEncounters(int page, int pageSize);
        Result<PagedResult<EncounterDto>> GetNearbyHidden(int page, int pageSize, int userId);
        Result<PagedResult<EncounterDto>> GetNearby(int page, int pageSize, int userId);

    }
}
