using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public class PublicKeypoint : Entity
{
    public PublicKeypoint()
    {
    }

    public PublicKeypoint(string name, double latitude, double longitude, string? description, string? image)
    {
        Validate(name, latitude, longitude);

        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Description = description;
        Image = image;
    }

    public string Name { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string? Description { get; init; }
    public string? Image { get; set; }

    private static void Validate(string name, double latitude, double longitude)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid name");
        if (latitude is > 90 or < -90) throw new ArgumentException("Invalid latitude");
        if (longitude is > 180 or < -180) throw new ArgumentException("Invalid longitude");
    }
}