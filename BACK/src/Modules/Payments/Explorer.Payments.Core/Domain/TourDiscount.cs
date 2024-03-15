using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payments.Core.Domain;

public class TourDiscount: Entity
{
    public TourDiscount(){}

    public TourDiscount(int discountId, int tourId)
    {
        DiscountId = discountId;
        TourId = tourId;
    }

    public long DiscountId { get; init; }
    public int TourId { get; init; }
}