using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface IPublicKeypointRepository : ICrudRepository<PublicKeypoint>
{
    PagedResult<PublicKeypoint> GetPagedInRange(int page, int pageSize, double longitude, double latitude,
        double radius);
}