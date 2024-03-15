namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface ITouristEquipmentRepository
{
    TouristEquipment GetByTouristAndEquipment(long touristId, long equipmentId);
    IEnumerable<TouristEquipment> GetAll();
}