using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using FluentResults;

namespace Explorer.Encounters.Core.Domain.RepositoryInterfaces
{
    public interface IKeypointEncounterRepository: ICrudRepository<KeypointEncounter>
    {
        PagedResult<KeypointEncounter> GetPagedByKeypoint(int page, int pageSize, long keypointId);
        List<KeypointEncounter> GetAllByKeypoint(long keypointId);
        KeypointEncounter Get(long keypointEncounterId);
    }
}
