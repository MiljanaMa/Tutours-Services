using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public class Equipment : Entity
{
    public Equipment(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        Name = name;
        Description = description;
    }

    public string Name { get; init; }
    public string? Description { get; init; }
    public ICollection<TourEquipment> TourEquipments { get; set; }
}