using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using FluentResults;


namespace Explorer.Encounters.API.Public
{
    public interface IKeypointEncounterService
    {
        Result<PagedResult<KeypointEncounterDto>> GetPaged(int page, int pageSize);
        Result<KeypointEncounterDto> Create(KeypointEncounterDto encounter);
        Result<KeypointEncounterDto> Update(KeypointEncounterDto encounter);
        Result UpdateEncountersLocation(LocationDto location, int keypointId);
        Result<PagedResult<KeypointEncounterDto>> GetPagedByKeypoint(int page, int pageSize, long keypointId);
        Result Delete(int id);
        Result DeleteKeypointEncounters(int keypointId);
    }
}
