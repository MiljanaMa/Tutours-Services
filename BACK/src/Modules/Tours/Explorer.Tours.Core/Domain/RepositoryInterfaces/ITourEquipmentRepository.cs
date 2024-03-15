namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface ITourEquipmentRepository
{
    List<Equipment> GetEquipmentForTour(int tourId);
    void AddEquipmentToTour(TourEquipment tourEquipment);
    void RemoveEquipmentFromTour(TourEquipment tourEquipment);
}