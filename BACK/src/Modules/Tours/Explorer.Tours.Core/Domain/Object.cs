using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Enum;

namespace Explorer.Tours.Core.Domain;

public class Object : Entity
{
    public Object()
    {
    }

    public Object(string name, string description, double latitude, double longitude, string? image, Category category,
        ObjectStatus status)
    {
        Validate(name, description, latitude, longitude);

        Name = name;
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        Image = image;
        Category = category;
        Status = status;
    }

    public string Name { get; init; }
    public string Description { get; init; }
    public string? Image { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public Category Category { get; init; }
    public ObjectStatus Status { get; set; }

    private void Validate(string name, string description, double latitude, double longitude)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Name.");
        if (latitude is > 90 or < -90) throw new ArgumentException("Invalid latitude");
        if (longitude is > 180 or < -180) throw new ArgumentException("Invalid longitude");
    }
}