using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface IKeypointRepository : ICrudRepository<Keypoint>
{
    public PagedResult<Keypoint> GetByTour(int page, int pageSize, int tourId);
    public IEnumerable<int?> GetNextPositions(long tourId, int? currentPosition);

    public IEnumerable<Keypoint> GetByTourAndPosition(long tourId, int? currentPosition);
}