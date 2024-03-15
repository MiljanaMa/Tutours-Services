using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payments.Core.Domain;

public class TourPurchaseToken : Entity
{
    public TourPurchaseToken(int tourId, int touristId)
    {
        TourId = tourId;
        TouristId = touristId;
    }

    public int TourId { get; init; }
    public int TouristId { get; init; }
}