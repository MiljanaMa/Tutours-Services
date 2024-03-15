using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface ITouristPositionRepository : ICrudRepository<TouristPosition>
{
    TouristPosition GetByUser(long userId);
}