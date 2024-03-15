using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class TouristEquipmentRepository : ITouristEquipmentRepository
{
    private readonly ToursContext _dbContext;

    public TouristEquipmentRepository(ToursContext dbContext)
    {
        _dbContext = dbContext;
    }

    public TouristEquipment GetByTouristAndEquipment(long touristId, long equipmentId)
    {
        return _dbContext.TouristEquipment.FirstOrDefault(teq =>
            teq.TouristId == touristId && teq.EquipmentId == equipmentId);
    }

    public IEnumerable<TouristEquipment> GetAll()
    {
        return _dbContext.TouristEquipment.ToList();
    }
}