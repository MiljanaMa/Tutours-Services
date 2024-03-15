using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public class Keypoint : Entity
{
    public Keypoint()
    {
    }

    public Keypoint(long tourId, string name, double latitude, double longitude, string? description, int? position,
        string? image, string? secret)
    {
        Validate(name, latitude, longitude, position);

        TourId = tourId;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Description = description;
        Position = position;
        Image = image;
        Secret = secret;
    }

    public long TourId { get; }
    public Tour Tour { get; }
    public string Name { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string? Description { get; init; }
    public int? Position { get; init; }
    public string? Image { get; set; }
    public string? Secret { get; set; }

    private static void Validate(string name, double latitude, double longitude, int? position)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid name");
        if (latitude is > 90 or < -90) throw new ArgumentException("Invalid latitude");
        if (longitude is > 180 or < -180) throw new ArgumentException("Invalid longitude");
        if (position < 1) throw new ArgumentException("Invalid position");
    }
}