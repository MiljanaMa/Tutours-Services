
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.Core.Domain.Enums;
using Explorer.Tours.Core.Domain;

namespace Explorer.Encounters.Core.Domain.RepositoryInterfaces
{
    public interface IEncounterRepository : ICrudRepository<Encounter>
    {

        public PagedResult<Encounter> GetApproved(int page, int pageSize);
        PagedResult<Encounter> GetApprovedByStatus(int page, int pageSize, EncounterStatus status);
        IEnumerable<Encounter> GetApprovedByStatusAndType(EncounterStatus status, EncounterType type);
        public PagedResult<Encounter> GetByUser(int page, int pageSize, long userId);
        public PagedResult<Encounter> GetTouristCreatedEncounters(int page, int pageSize);
        PagedResult<Encounter> GetNearbyByType(int page, int pageSize, double longitude, double latitude, EncounterType type);
        PagedResult<Encounter> GetNearby(int page, int pageSize, double longitude, double latitude);

    }
}
