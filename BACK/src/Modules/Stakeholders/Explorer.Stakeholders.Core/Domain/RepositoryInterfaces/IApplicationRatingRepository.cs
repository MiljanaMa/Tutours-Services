using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IApplicationRatingRepository : ICrudRepository<ApplicationRating>
{
    ApplicationRating GetByUser(long userId);
}