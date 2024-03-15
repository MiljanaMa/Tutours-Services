using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface IObjectRepository : ICrudRepository<Object>
{
    PagedResult<Object> GetPublicPaged(int page, int pageSize);
    PagedResult<Object> GetPublicPagedInRange(int page, int pageSize, double longitude, double latitude, double radius);
}