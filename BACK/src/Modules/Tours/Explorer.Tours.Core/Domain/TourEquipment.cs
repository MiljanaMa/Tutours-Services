using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public class TourEquipment : Entity
{
    public TourEquipment(long tourId, long equipmentId)
    {
        TourId = tourId;
        EquipmentId = equipmentId;
    }

    public long TourId { get; init; }
    public long EquipmentId { get; init; }
}