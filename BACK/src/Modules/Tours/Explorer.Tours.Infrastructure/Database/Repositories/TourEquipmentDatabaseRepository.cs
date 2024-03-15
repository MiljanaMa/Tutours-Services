using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class TourEquipmentDatabaseRepository : ITourEquipmentRepository
{
    private readonly ToursContext _dbContext;

    public TourEquipmentDatabaseRepository(ToursContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddEquipmentToTour(TourEquipment tourEquipment)
    {
        _dbContext.TourEquipments.Add(tourEquipment);
        _dbContext.SaveChanges();
    }

    public List<Equipment> GetEquipmentForTour(int tourId)
    {
        var equipmentIds = _dbContext.TourEquipments.Where(te => te.TourId == tourId).Select(te => te.EquipmentId)
            .ToList();
        if (!equipmentIds.Any()) return new List<Equipment>();
        var equipment = _dbContext.Equipment.Where(e => equipmentIds.Contains((int)e.Id)).ToList();

        return equipment;
    }

    public void RemoveEquipmentFromTour(TourEquipment tourEquipment)
    {
        if (tourEquipment == null) throw new ArgumentNullException(nameof(tourEquipment));

        _dbContext.TourEquipments.Remove(tourEquipment);
        _dbContext.SaveChanges();
    }
}