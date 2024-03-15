namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface IEquipmentRepository
{
    IEnumerable<Equipment> GetAll();
}