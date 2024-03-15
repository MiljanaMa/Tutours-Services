using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces;

public interface ITourPurchaseTokenRepository : ICrudRepository<TourPurchaseToken>
{
    void AddRange(List<TourPurchaseToken> tokens);
    TourPurchaseToken GetByTourAndTourist(int tourId, int touristId);
    bool CheckIfPurchased(long tourId, long touristId);
}